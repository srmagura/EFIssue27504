namespace DbApplicationImpl;

public class EfProductPhotoRepository : Repository<AppDataContext>, IProductPhotoRepository
{
    public EfProductPhotoRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public void Add(ProductPhoto productPhoto)
    {
        var dbe = DbMapper.ToDb<DbProductPhoto>(productPhoto);
        Context.ProductPhotos.Add(dbe);
    }

    public async Task<ProductPhoto?> GetAsync(ProductPhotoId id)
    {
        var dbe = await Context.ProductPhotos.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<ProductPhoto>(dbe);
    }
}
