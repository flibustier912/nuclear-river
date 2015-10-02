﻿using NuClear.AdvancedSearch.Common.Metadata.Model;

namespace NuClear.CustomerIntelligence.Domain.Model.Erm
{
    public sealed class Account : IErmObject
    {
        public Account()
        {
            IsActive = true;
        }

        public long Id { get; set; }

        public decimal Balance { get; set; }

        public long BranchOfficeOrganizationUnitId { get; set; }
        
        public long LegalPersonId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Account && IdentifiableObjectEqualityComparer<Account>.Default.Equals(this, (Account)obj);
        }

        public override int GetHashCode()
        {
            return IdentifiableObjectEqualityComparer<Account>.Default.GetHashCode(this);
        }
    }
}