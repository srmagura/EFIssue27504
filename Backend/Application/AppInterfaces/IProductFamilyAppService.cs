using ITI.Baseline.Util;

namespace AppInterfaces;

public interface IProductFamilyAppService
{
    Task<FilteredList<ProductFamilyDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    );

    Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

    Task<ProductFamilyId> AddAsync(OrganizationId organizationId, string name);

    Task SetNameAsync(ProductFamilyId id, string name);
    Task SetActiveAsync(ProductFamilyId id, bool active);
}
