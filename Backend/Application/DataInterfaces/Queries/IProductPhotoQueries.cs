using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DataInterfaces.Queries
{
    public interface IProductPhotoQueries
    {
        Task<ProductPhotoDto?> GetAsync(ProductPhotoId id);

        Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

        Task<FilteredList<ProductPhotoSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );
    }
}
