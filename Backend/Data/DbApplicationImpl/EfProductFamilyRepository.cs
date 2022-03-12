namespace DbApplicationImpl;

public class EfProductFamilyRepository : Repository<AppDataContext>, IProductFamilyRepository
{
    public EfProductFamilyRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public void Add(ProductFamily productFamily)
    {
        var dbe = DbMapper.ToDb<DbProductFamily>(productFamily);
        Context.ProductFamilies.Add(dbe);
    }

    public async Task<ProductFamily?> GetAsync(ProductFamilyId id)
    {
        var dbe = await Context.ProductFamilies.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<ProductFamily>(dbe);
    }
}
