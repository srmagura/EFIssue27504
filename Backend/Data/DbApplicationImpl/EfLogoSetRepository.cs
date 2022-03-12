namespace DbApplicationImpl;

public class EfLogoSetRepository : Repository<AppDataContext>, ILogoSetRepository
{
    public EfLogoSetRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<LogoSet?> GetAsync(LogoSetId id)
    {
        var dbe = await Context.LogoSets.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<LogoSet>(dbe);
    }

    public void Add(LogoSet logoSet)
    {
        var dbe = DbMapper.ToDb<DbLogoSet>(logoSet);
        Context.LogoSets.Add(dbe);
    }
}
