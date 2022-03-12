namespace DbApplicationImpl;

public class EfProjectPublicationQueries : Queries<AppDataContext>, IProjectPublicationQueries
{
    private readonly IMapper _mapper;

    public EfProjectPublicationQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<ProjectPublicationSummaryDto?> GetAsync(ProjectPublicationId id)
    {
        return Context.ProjectPublications
            .Include(p => p.Reports) // TODO:SAM
            .Include(p => p.PublishedBy)
            .Where(i => i.Id == id.Guid)
            .ProjectToDtoAsync<DbProjectPublication, ProjectPublicationSummaryDto>(_mapper);
    }

    public Task<ProjectPublicationSummaryDto[]> ListAsync(ProjectId projectId)
    {
        return Context.ProjectPublications
            .Include(p => p.Reports)
            .Include(p => p.PublishedBy)
            .Where(p => p.ProjectId == projectId.Guid)
            .OrderByDescending(p => p.DateCreatedUtc)
            .ProjectToDtoArrayAsync<DbProjectPublication, ProjectPublicationSummaryDto>(_mapper);
    }

    public Task<int> CountAsync(ProjectId id)
    {
        return Context.ProjectPublications
            .Where(p => p.ProjectId == id.Guid)
            .CountAsync();
    }
}
