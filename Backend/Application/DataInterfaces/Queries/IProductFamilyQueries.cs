using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DataInterfaces.Queries
{
    public interface IProductFamilyQueries
    {
        Task<FilteredList<ProductFamilyDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );

        Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);
    }
}
