using ITI.Baseline.Util;

namespace AppInterfaces;

public interface ITermsDocumentAppService
{
    Task<string?> GetPdfAsync(TermsDocumentId id, Stream outputStream); // returns "application/pdf" or null

    Task<FilteredList<TermsDocumentSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter
    );

    Task<TermsDocumentId> AddAsync(
        OrganizationId organizationId,
        Stream stream
    );

    Task SetActiveAsync(TermsDocumentId id, bool active);
}
