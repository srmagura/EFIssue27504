using ITI.Baseline.Util;

namespace AppInterfaces
{
    public interface IProductPhotoAppService
    {
        Task<ProductPhotoDto?> GetAsync(ProductPhotoId id);
        Task<string?> GetImageAsync(ProductPhotoId id, Stream outputStream); // returns the image file type

        Task<FilteredList<ProductPhotoSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );

        Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

        Task<ProductPhotoId> AddAsync(
            OrganizationId organizationId,
            string name,
            Stream stream,
            string fileType
        );

        Task SetNameAsync(ProductPhotoId id, string name);
        Task SetPhotoAsync(ProductPhotoId id, Stream stream, string fileType);
        Task SetActiveAsync(ProductPhotoId id, bool active);
    }
}
