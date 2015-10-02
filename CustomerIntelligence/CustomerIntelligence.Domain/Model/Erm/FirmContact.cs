﻿using NuClear.AdvancedSearch.Common.Metadata.Model;

namespace NuClear.CustomerIntelligence.Domain.Model.Erm
{
    public sealed class FirmContact : IErmObject
    {
        public long Id { get; set; }

        public int ContactType { get; set; }

        public long? FirmAddressId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FirmContact && IdentifiableObjectEqualityComparer<FirmContact>.Default.Equals(this, (FirmContact)obj);
        }

        public override int GetHashCode()
        {
            return IdentifiableObjectEqualityComparer<FirmContact>.Default.GetHashCode(this);
        }
    }
}