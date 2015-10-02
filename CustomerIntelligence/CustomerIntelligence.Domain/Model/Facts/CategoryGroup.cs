﻿using NuClear.AdvancedSearch.Common.Metadata.Model;

namespace NuClear.CustomerIntelligence.Domain.Model.Facts
{
    public sealed class CategoryGroup : IFactObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public float Rate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CategoryGroup && IdentifiableObjectEqualityComparer<CategoryGroup>.Default.Equals(this, (CategoryGroup)obj);
        }

        public override int GetHashCode()
        {
            return IdentifiableObjectEqualityComparer<CategoryGroup>.Default.GetHashCode(this);
        }
    }
}