namespace DbApplicationImpl;

public class EfProjectRepository : Repository<AppDataContext>, IProjectRepository
{
    public EfProjectRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<Project?> GetAsync(ProjectId id)
    {
        var dbe = await Context.Projects.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<Project>(dbe);
    }

    public void Add(Project project)
    {
        var dbe = DbMapper.ToDb<DbProject>(project);
        Context.Projects.Add(dbe);
    }
}
