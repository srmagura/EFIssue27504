using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DataInterfaces.Queries
{
    public interface ITermsDocumentQueries
    {
        Task<TermsDocumentDto?> GetAsync(TermsDocumentId id);

        Task<FilteredList<TermsDocumentSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter
        );

        Task<int> LastNumberAsync(OrganizationId organizationId);
    }
}
