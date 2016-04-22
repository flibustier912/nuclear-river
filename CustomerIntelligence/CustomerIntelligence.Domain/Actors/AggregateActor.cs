﻿using System.Collections.Generic;
using System.Linq;

using NuClear.CustomerIntelligence.Domain.Commands;
using NuClear.Replication.Core.API.Actors;
using NuClear.River.Common.Metadata;

namespace NuClear.CustomerIntelligence.Domain.Actors
{
    public class AggregateActor : IAggregateActor
    {
        private readonly LeafToRootActor _leafToRootActor;
        private readonly RootToLeafActor _rootToLeafActor;

        public AggregateActor(IAggregateRootActor aggregateRootActor)
        {
            _leafToRootActor = new LeafToRootActor(aggregateRootActor);
            _rootToLeafActor = new RootToLeafActor(aggregateRootActor);
        }

        public IReadOnlyCollection<IEvent> ExecuteCommands(IReadOnlyCollection<ICommand> commands)
        {
            var events = new List<IEvent>();

            var destroyAggregateCommands = commands.OfType<DestroyAggregateCommand>().Distinct().ToArray();
            events.AddRange(_leafToRootActor.ExecuteCommands(destroyAggregateCommands));

            var initializeAggregateCommands = commands.OfType<InitializeAggregateCommand>().Distinct().ToArray();
            events.AddRange(_rootToLeafActor.ExecuteCommands(initializeAggregateCommands));

            var recalculateAggregateCommands = commands.OfType<RecalculateAggregateCommand>().Distinct().ToArray();
            events.AddRange(_rootToLeafActor.ExecuteCommands(recalculateAggregateCommands));

            var recalculateEntityCommands = commands.OfType<RecalculateEntityCommand>().Distinct().ToArray();
            events.AddRange(_rootToLeafActor.ExecuteCommands(recalculateEntityCommands));

            return events;
        }
    }
}