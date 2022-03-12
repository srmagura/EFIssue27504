namespace DbApplicationImpl;

public class EfImportQueries : Queries<AppDataContext>, IImportQueries
{
    private readonly IMapper _mapper;

    public EfImportQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<ImportDto?> GetAsync(ImportId id)
    {
        return Context.PageImports
            .Where(i => i.Id == id.Guid)
            .ProjectToDtoAsync<DbPageImport, ImportDto>(_mapper);
    }

    public Task<ImportDto[]> ListAsync(ProjectId projectId, int skip, int take)
    {
        return Context.PageImports
            .Where(i => i.ProjectId == projectId.Guid)
            .OrderByDescending(i => i.DateCreatedUtc)
            .Skip(skip)
            .Take(take)
            .ProjectToDtoArrayAsync<DbPageImport, ImportDto>(_mapper);
    }
}
