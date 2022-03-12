using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DbApplicationImpl;

public class EfTermsDocumentQueries : Queries<AppDataContext>, ITermsDocumentQueries
{
    private readonly IMapper _mapper;

    public EfTermsDocumentQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<TermsDocumentDto?> GetAsync(TermsDocumentId id)
    {
        var q = Context.TermsDocuments
            .Where(p => p.Id == id.Guid);

        return _mapper.ProjectToDtoAsync<DbTermsDocument, TermsDocumentDto>(q);
    }

    public async Task<FilteredList<TermsDocumentSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter
    )
    {
        var q = Context.TermsDocuments.Where(p => p.OrganizationId == organizationId.Guid);

        switch (activeFilter)
        {
            case ActiveFilter.ActiveOnly:
                q = q.Where(p => p.IsActive);
                break;
            case ActiveFilter.InactiveOnly:
                q = q.Where(p => !p.IsActive);
                break;
            case ActiveFilter.All:
                break;
        }

        var count = await q.CountAsync();

        q = q.OrderByDescending(p => p.DateCreatedUtc)
            .Skip(skip)
            .Take(take);

        var items = await _mapper.ProjectToDtoArrayAsync<DbTermsDocument, TermsDocumentSummaryDto>(q);

        return new FilteredList<TermsDocumentSummaryDto>(items, count);
    }

    public async Task<int> LastNumberAsync(OrganizationId organizationId)
    {
        var q = Context.TermsDocuments
            .Where(p => p.OrganizationId == organizationId.Guid)
            .Select(p => (int?)p.Number);

        return await q.MaxAsync() ?? 0;
    }
}
