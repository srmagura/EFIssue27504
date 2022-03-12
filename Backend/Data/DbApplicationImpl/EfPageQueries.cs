using AppDTOs.Enumerations;

namespace DbApplicationImpl;

public class EfPageQueries : Queries<AppDataContext>, IPageQueries
{
    private readonly IMapper _mapper;

    public EfPageQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<PageDto?> GetAsync(PageId id)
    {
        var q = Context.Pages
            .Where(p => p.Id == id.Guid);

        return _mapper.ProjectToDtoAsync<DbPage, PageDto>(q);
    }

    public Task<List<PageSummaryDto>> ListAsync(ProjectId id, ActiveFilter activeFilter)
    {
        var q = Context.Pages.AsQueryable();

        switch (activeFilter)
        {
            case ActiveFilter.ActiveOnly:
                q = q.Where(p => p.IsActive && p.ProjectId == id.Guid);
                break;
            case ActiveFilter.InactiveOnly:
                q = q.Where(p => !p.IsActive && p.ProjectId == id.Guid);
                break;
            case ActiveFilter.All:
                q = q.Where(p => p.ProjectId == id.Guid);
                break;
        }

        q = q.OrderBy(p => p.Index);

        return _mapper.ProjectToDtoListAsync<DbPage, PageSummaryDto>(q);
    }
}
