using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using NuClear.AdvancedSearch.Replication.API.Model;
using NuClear.Storage.Readings;
using NuClear.Storage.Specifications;

namespace NuClear.AdvancedSearch.Replication.CustomerIntelligence.Transforming.Metadata
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    internal static class AggregateInfoBuilder
    {
        public static AggregateInfoBuilder<TAggregate> OfType<TAggregate>() where TAggregate : class, ICustomerIntelligenceObject, IIdentifiable
        {
            return new AggregateInfoBuilder<TAggregate>();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    internal class AggregateInfoBuilder<TAggregate> where TAggregate : class, ICustomerIntelligenceObject, IIdentifiable
    {
        private readonly List<IMetadataInfo> _valueObjects;
        private readonly Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<TAggregate>>> _mapToTargetSpecProvider =
            ids => new MapSpecification<IQuery, IQueryable<TAggregate>>(q => q.For(new FindSpecification<TAggregate>(x => ids.Contains(x.Id))));

        private Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<TAggregate>>> _mapToSourceSpecProvider;

        public AggregateInfoBuilder()
        {
            _valueObjects = new List<IMetadataInfo>();
        }

        public IAggregateInfo Build()
        {
            return new AggregateInfo<TAggregate>(_mapToSourceSpecProvider, _mapToTargetSpecProvider, _valueObjects);
        }

        public AggregateInfoBuilder<TAggregate> HasSource(Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<TAggregate>>> mapToSourceSpecProvider)
        {
            _mapToSourceSpecProvider = mapToSourceSpecProvider;
            return this;
        }

        public AggregateInfoBuilder<TAggregate> HasValueObject<TValueObject>(
            Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<TValueObject>>> mapToSourceSpecProvider,
            Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<TValueObject>>> mapToTargetSpecProvider)
        {
            _valueObjects.Add(new ValueObjectInfo<TValueObject>(mapToSourceSpecProvider, mapToTargetSpecProvider));
            return this;
        }
    }
}