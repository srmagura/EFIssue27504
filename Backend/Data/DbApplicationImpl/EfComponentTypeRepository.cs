namespace DbApplicationImpl;

public class EfComponentTypeRepository : Repository<AppDataContext>, IComponentTypeRepository
{
    public EfComponentTypeRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public void Add(ComponentType componentType)
    {
        var dbe = DbMapper.ToDb<DbComponentType>(componentType);
        Context.ComponentTypes.Add(dbe);
    }

    public async Task<ComponentType?> GetAsync(ComponentTypeId componentTypeId)
    {
        var dbe = await Context.ComponentTypes.FirstOrDefaultAsync(p => p.Id == componentTypeId.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<ComponentType>(dbe);
    }
}
