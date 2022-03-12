namespace DbApplicationImpl;

public class EfSheetTypeRepository : Repository<AppDataContext>, ISheetTypeRepository
{
    public EfSheetTypeRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<SheetType?> GetAsync(SheetTypeId id)
    {
        var dbe = await Context.SheetTypes.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<SheetType>(dbe);
    }

    public void Add(SheetType sheetType)
    {
        var dbe = DbMapper.ToDb<DbSheetType>(sheetType);
        Context.SheetTypes.Add(dbe);
    }
}
