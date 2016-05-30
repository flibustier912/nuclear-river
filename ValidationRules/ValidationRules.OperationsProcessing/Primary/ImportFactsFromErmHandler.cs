﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using NuClear.Messaging.API.Processing;
using NuClear.Messaging.API.Processing.Actors.Handlers;
using NuClear.Messaging.API.Processing.Stages;
using NuClear.Replication.Core;
using NuClear.Replication.Core.Settings;
using NuClear.Replication.OperationsProcessing;
using NuClear.Replication.OperationsProcessing.Telemetry;
using NuClear.Replication.OperationsProcessing.Transports;
using NuClear.Telemetry;
using NuClear.Telemetry.Probing;
using NuClear.Tracing.API;
using NuClear.ValidationRules.OperationsProcessing.Identities.Flows;

namespace NuClear.ValidationRules.OperationsProcessing.Primary
{
    public sealed class ImportFactsFromErmHandler : IMessageProcessingHandler
    {
        private readonly IReplicationSettings _replicationSettings;
        private readonly IDataObjectsActorFactory _dataObjectsActorFactory;
        private readonly IEventSender _eventSender;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ITracer _tracer;
        private readonly ITelemetryPublisher _telemetryPublisher;

        public ImportFactsFromErmHandler(
            IReplicationSettings replicationSettings,
            IDataObjectsActorFactory dataObjectsActorFactory,
            IEventSender eventSender,
            ITelemetryPublisher telemetryPublisher,
            IEventDispatcher eventDispatcher,
            ITracer tracer)
        {
            _replicationSettings = replicationSettings;
            _dataObjectsActorFactory = dataObjectsActorFactory;
            _eventSender = eventSender;
            _telemetryPublisher = telemetryPublisher;
            _eventDispatcher = eventDispatcher;
            _tracer = tracer;
        }

        public IEnumerable<StageResult> Handle(IReadOnlyDictionary<Guid, List<IAggregatableMessage>> processingResultsMap)
        {
            try
            {
                var messages = processingResultsMap.SelectMany(pair => pair.Value)
                                                   .Cast<AggregatableMessage<ICommand>>()
                                                   .ToArray();

                Handle(processingResultsMap.Keys.ToArray(), messages.SelectMany(message => message.Commands).ToArray());

                var oldestEventTime = messages.Min(message => message.EventHappenedTime);
                _telemetryPublisher.Publish<PrimaryProcessingDelayIdentity>((long)(DateTime.UtcNow - oldestEventTime).TotalMilliseconds);

                return processingResultsMap.Keys.Select(bucketId => MessageProcessingStage.Handling.ResultFor(bucketId).AsSucceeded());
            }
            catch (Exception ex)
            {
                _tracer.Error(ex, "Error when import facts for ERM");
                return processingResultsMap.Keys.Select(bucketId => MessageProcessingStage.Handling.ResultFor(bucketId).AsFailed().WithExceptions(ex));
            }
        }

        private void Handle(IReadOnlyCollection<Guid> bucketIds, IReadOnlyCollection<ICommand> commands)
        {
            _tracer.Debug("Handing fact operations started");

            var events = new List<IEvent>();
            using (Probe.Create("ETL1 Transforming"))
            {
                // TODO: Can actors be executed in parallel? See https://github.com/2gis/nuclear-river/issues/76
                var actors = _dataObjectsActorFactory.Create();
                foreach (var actor in actors)
                {
                    var actorType = actor.GetType().GetFriendlyName();
                    using (Probe.Create("ETL1 Transforming", actorType))
                    {
                        _tracer.Debug($"Applying changes to target facts storage with actor {actorType}");
                        foreach (var batch in commands.CreateBatches(_replicationSettings.ReplicationBatchSize))
                        {
                            events.AddRange(actor.ExecuteCommands(batch));
                        }
                    }
                }
            }

            if (events.Count > 1000 * bucketIds.Count)
            {
                _tracer.Warn($"Messages produced huge events amount: from {bucketIds.Count} TUCs to {events.Count} commands\n" +
                             string.Join(", ", bucketIds));
            }

            _telemetryPublisher.Publish<ErmProcessedOperationCountIdentity>(commands.Count);

            // We always need to use different transaction scope to operate with operation sender because it has its own store
            using (var pushTransaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                              new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.Zero }))
            {
                _tracer.Debug("Pushing messages");
                DispatchEvents(events);
                pushTransaction.Complete();
            }

            _tracer.Debug("Handing fact operations finished");
        }

        private void DispatchEvents(IReadOnlyCollection<IEvent> events)
        {
            var dispatched = _eventDispatcher.Dispatch(events);
            foreach (var pair in dispatched)
            {
                _eventSender.Push(pair.Key, pair.Value);
            }

            _telemetryPublisher.Publish<AggregateEnqueuedOperationCountIdentity>(dispatched[CommonEventsFlow.Instance].Count);
        }
    }
}