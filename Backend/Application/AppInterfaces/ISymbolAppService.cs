using ITI.Baseline.Util;

namespace AppInterfaces
{
    public interface ISymbolAppService
    {
        Task<SymbolDto?> GetAsync(SymbolId id);

        Task<FilteredList<SymbolSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );

        Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

        Task<SymbolId> AddAsync(
            OrganizationId organizationId,
            string name,
            string svgText
        );

        Task SetNameAsync(SymbolId id, string name);
        Task SetSvgTextAsync(SymbolId id, string svgText);
        Task SetActiveAsync(SymbolId id, bool active);
    }
}
