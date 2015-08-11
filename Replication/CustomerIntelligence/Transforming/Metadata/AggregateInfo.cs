using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NuClear.AdvancedSearch.Replication.API.Model;
using NuClear.Storage.Readings;
using NuClear.Storage.Specifications;

namespace NuClear.AdvancedSearch.Replication.CustomerIntelligence.Transforming.Metadata
{
    internal sealed class AggregateInfo<T> : IAggregateInfo where T : class, IIdentifiable
    {
        private readonly Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<T>>> _mapToSourceSpecProvider;
        private readonly Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<T>>> _mapToTargetSpecProvider;

        public AggregateInfo(
            Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<T>>> mapToSourceSpecProvider,
            Func<IReadOnlyCollection<long>, MapSpecification<IQuery, IQueryable<T>>> mapToTargetSpecProvider,
            IEnumerable<IMetadataInfo> valueObjects = null)
        {
            _mapToSourceSpecProvider = mapToSourceSpecProvider;
            _mapToTargetSpecProvider = mapToTargetSpecProvider;
            ValueObjects = valueObjects;
        }

        public Type Type
        {
            get { return typeof(T); }
        }

        public MapToObjectsSpecProvider MapToSourceSpecProvider
        {
            get
            {
                return ids =>
                       {
                           if (!ids.Any())
                           {
                               return new MapSpecification<IQuery, IEnumerable>(q => Enumerable.Empty<T>());
                           }

                           var mapToSourceSpec = _mapToSourceSpecProvider(ids);
                           return new MapSpecification<IQuery, IEnumerable>(mapToSourceSpec);
                       };
            }
        }

        public MapToObjectsSpecProvider MapToTargetSpecProvider
        {
            get
            {
                return ids =>
                       {
                           if (!ids.Any())
                           {
                               return new MapSpecification<IQuery, IEnumerable>(q => Enumerable.Empty<T>());
                           }

                           var mapToTargetSpec = _mapToTargetSpecProvider(ids);
                           return new MapSpecification<IQuery, IEnumerable>(mapToTargetSpec);
                       };
            }
        }

        public IEnumerable<IMetadataInfo> ValueObjects { get; private set; }
    }
}