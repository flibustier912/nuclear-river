using NuClear.AdvancedSearch.Replication.Model;

namespace NuClear.AdvancedSearch.Replication.CustomerIntelligence.Model.Erm
{
    public sealed class CategoryOrganizationUnit : IIdentifiableObject, IErmObject
    {
        public CategoryOrganizationUnit()
        {
            IsActive = true;
        }

        public long Id { get; set; }

        public long CategoryId { get; set; }

        public long CategoryGroupId { get; set; }

        public long OrganizationUnitId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CategoryOrganizationUnit && IdentifiableObjectEqualityComparer<CategoryOrganizationUnit>.Default.Equals(this, (CategoryOrganizationUnit)obj);
        }

        public override int GetHashCode()
        {
            return IdentifiableObjectEqualityComparer<CategoryOrganizationUnit>.Default.GetHashCode(this);
        }
    }
}