namespace DbApplicationImpl;

public class EfImportRepository : Repository<AppDataContext>, IImportRepository
{
    public EfImportRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<Import?> GetAsync(ImportId id)
    {
        var dbe = await Context.PageImports.FirstOrDefaultAsync(i => i.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<Import>(dbe);
    }

    public void Add(Import import)
    {
        var dbe = DbMapper.ToDb<DbPageImport>(import);
        Context.Add(dbe);
    }
}
