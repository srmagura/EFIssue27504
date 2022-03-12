using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DataInterfaces.Queries
{
    public interface ISymbolQueries
    {
        Task<SymbolDto?> GetAsync(SymbolId id);

        Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

        Task<FilteredList<SymbolSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );
    }
}
