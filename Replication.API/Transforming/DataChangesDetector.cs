﻿using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using NuClear.Storage.Readings;
using NuClear.Storage.Specifications;

namespace NuClear.AdvancedSearch.Replication.API.Transforming
{
    internal class DataChangesDetector<TFilter, TOutput>
    {
        private readonly IQuery _query;
        private readonly MapToObjectsSpecProvider<TFilter, TOutput> _sourceProvider;
        private readonly MapToObjectsSpecProvider<TFilter, TOutput> _targetProvider;

        public DataChangesDetector(
            MapToObjectsSpecProvider<TFilter, TOutput> sourceProvider,
            MapToObjectsSpecProvider<TFilter, TOutput> targetProvider,
            IQuery query)
        {
            _sourceProvider = sourceProvider;
            _targetProvider = targetProvider;
            _query = query;
        }

        public MergeResult<TCompared> DetectChanges<TCompared>(MapSpecification<IEnumerable<TOutput>, IEnumerable<TCompared>> mapSpec, FindSpecification<TFilter> specification)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                var sourceObjects = _sourceProvider.Invoke(specification).Map(_query);
                var targetObjects = _targetProvider.Invoke(specification).Map(_query);

                var result = MergeTool.Merge(
                    mapSpec.Map(sourceObjects).ToArray(),
                    mapSpec.Map(targetObjects).ToArray());

                scope.Complete();

                return result;
            }
        }
    }
}