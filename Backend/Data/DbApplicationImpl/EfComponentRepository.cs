namespace DbApplicationImpl;

public class EfComponentRepository : Repository<AppDataContext>, IComponentRepository
{
    public EfComponentRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public void Add(Component component)
    {
        var dbe = DbMapper.ToDb<DbComponent>(component);
        Context.Components.Add(dbe);
    }

    public void AddVersion(ComponentVersion version)
    {
        var dbe = DbMapper.ToDb<DbComponentVersion>(version);
        Context.ComponentVersions.Add(dbe);
    }

    public async Task<Component?> GetAsync(ComponentId id)
    {
        var dbe = await Context.Components.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<Component>(dbe);
    }

    public async Task<ComponentVersion?> GetVersionAsync(ComponentVersionId id)
    {
        var dbe = await Context.ComponentVersions.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<ComponentVersion>(dbe);
    }

    public async Task<ComponentVersion[]> ListVersionsByIdAsync(ComponentVersionId[] ids)
    {
        var idGuids = ids.Select(id => id.Guid).ToArray();

        var dbEntities = await Context.ComponentVersions
            .Where(v => idGuids.Contains(v.Id))
            .ToArrayAsync();

        return dbEntities.Select(dbe => DbMapper.ToEntity<ComponentVersion>(dbe)).ToArray();
    }
}
