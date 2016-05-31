﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

using NuClear.Replication.Core;
using NuClear.Replication.Core.Actors;
using NuClear.Replication.Core.DataObjects;
using NuClear.Storage.API.Readings;
using NuClear.ValidationRules.Storage.Model.Aggregates;

using Version = NuClear.ValidationRules.Storage.Model.Messages.Version;

namespace NuClear.ValidationRules.Replication.Actors
{
    public sealed class AdvertisementAmountRestrictionIntegrityActor : IActor
    {
        private const int MessageTypeId = 2;

        private readonly IQuery _query;
        private readonly IBulkRepository<Version.ValidationResult> _repository;

        public AdvertisementAmountRestrictionIntegrityActor(IQuery query, IBulkRepository<Version.ValidationResult> repository)
        {
            _query = query;
            _repository = repository;
        }

        public IReadOnlyCollection<IEvent> ExecuteCommands(IReadOnlyCollection<ICommand> commands)
        {
            // todo: привести в соответствие с созданием новой версии
            var currentVersion = _query.For<Version>().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;

            IReadOnlyCollection<Version.ValidationResult> sourceObjects;
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                // Запрос к данным источника посылаем вне транзакции, большой беды от этого быть не должно.
                sourceObjects = GetValidationResults(_query, currentVersion).ToArray();

                // todo: удалить, добавлено с целью отладки
                sourceObjects = sourceObjects.Where(x => x.PeriodStart >= DateTime.Parse("2016-06-01")).ToArray();

                scope.Complete();
            }

            // Данные в целевых таблицах меняем в одной большой транзакции (сейчас она управляется из хендлера)
            var targetObjects = _query.For<Version.ValidationResult>().Where(x => x.MessageType == MessageTypeId && x.VersionId == 0).ToArray();
            _repository.Delete(targetObjects);
            _repository.Create(sourceObjects);

            return Array.Empty<IEvent>();
        }

        private static IQueryable<Version.ValidationResult> GetValidationResults(IQuery query, long version)
        {
            // todo: Заказы тут вообще не при делах.
            // Нужно доработать схему, чтобы как минимум поле OrderId стало не обязательным, а как максимум появилась система тегов.

            var ruleResults = from position in query.For<Position>().Where(x => x.IsControlledByAmount)
                              join restriction in query.For<AdvertisementAmountRestriction>().Where(x => x.MissingMinimalRestriction) on position.Id equals restriction.PositionId
                              join pp in query.For<PricePeriod>() on restriction.PriceId equals pp.PriceId
                              join period in query.For<Period>() on new { pp.Start, pp.OrganizationUnitId } equals new { period.Start, period.OrganizationUnitId }
                              join op in query.For<OrderPeriod>() on new { pp.Start, pp.OrganizationUnitId } equals new { op.Start, op.OrganizationUnitId }
                              select new Version.ValidationResult
                                  {
                                      MessageType = MessageTypeId,
                                      MessageParams = new XDocument(new XElement("empty", new XAttribute("name", position.Name))),
                                      OrderId = op.OrderId,
                                      PeriodStart = period.Start,
                                      PeriodEnd = period.End,
                                      OrganizationUnitId = pp.OrganizationUnitId,
                                      Result = 1,
                                      VersionId = version
                                  };

            return ruleResults;
        }
    }
}