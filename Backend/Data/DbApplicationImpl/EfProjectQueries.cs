using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DbApplicationImpl;

public class EfProjectQueries : Queries<AppDataContext>, IProjectQueries
{
    private readonly IMapper _mapper;

    public EfProjectQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public async Task<ProjectDto?> GetAsync(ProjectId id)
    {
        var dbProject = await Context.Projects
            .Include(p => p.DesignerLockedBy)
            .Include(p => p.ReportOptions.TermsDocument)
            .Where(p => p.Id == id.Guid)
            .FirstOrDefaultAsync();

        // TODO:SAM Change back to projection and remove Includes when new AutoMapper version released (bug is fixed in prerelease build)
        if (dbProject == null) return null;
        return _mapper.Map<ProjectDto>(dbProject);
    }

    public async Task<UserId?> GetDesignerLockedByIdAsync(ProjectId id)
    {
        var guid = await Context.Projects
            .Where(p => p.Id == id.Guid)
            .Select(p => p.DesignerLockedById)
            .FirstOrDefaultAsync();

        if (guid == null) return null;

        return new UserId(guid.Value);
    }

    public async Task<FilteredList<ProjectSummaryDto>> ListAsync(OrganizationId organizationId, int skip, int take, ActiveFilter activeFilter, string? search)
    {
        var q = Context.Projects.Where(p => p.OrganizationId == organizationId.Guid);

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

        if (search.HasValue())
        {
            q = q.Where(p =>
                p.Name.Contains(search) ||
                p.ShortName.Contains(search) ||
                p.Address.Line1!.Contains(search) ||
                p.Address.City!.Contains(search) ||
                p.Address.State!.Contains(search)
            );
        }

        var count = await q.CountAsync();

        q = q.OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take);

        var items = await _mapper.ProjectToDtoArrayAsync<DbProject, ProjectSummaryDto>(q);

        return new FilteredList<ProjectSummaryDto>(items, count);
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        var q = Context.Projects.Where(p => p.OrganizationId == organizationId.Guid);

        return q.AllAsync(p => p.Name != name);
    }
}
