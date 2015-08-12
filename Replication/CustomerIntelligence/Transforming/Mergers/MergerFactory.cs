using System;

using NuClear.AdvancedSearch.Replication.API.Transforming;

namespace NuClear.AdvancedSearch.Replication.CustomerIntelligence.Transforming.Mergers
{
    internal static class MergerFactory
    {
        public static IValueObjectMerger CreateValueObjectMerger(IMetadataInfo valueObjectInfo)
        {
            var createType = typeof(ValueObjectMerger<>).MakeGenericType(valueObjectInfo.Type);
            return (IValueObjectMerger)Activator.CreateInstance(createType);
        }
    }
}