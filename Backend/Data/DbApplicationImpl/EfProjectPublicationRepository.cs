namespace DbApplicationImpl;

public class EfProjectPublicationRepository : Repository<AppDataContext>, IProjectPublicationRepository
{
    public EfProjectPublicationRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<ProjectPublication?> GetAsync(ProjectPublicationId id)
    {
        var dbe = await Context.ProjectPublications.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<ProjectPublication>(dbe);
    }

    public void Add(ProjectPublication projectPublication)
    {
        var dbe = DbMapper.ToDb<DbProjectPublication>(projectPublication);
        Context.Add(dbe);
    }
}
