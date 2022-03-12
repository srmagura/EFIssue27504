namespace DbApplicationImpl
{
    public class EfProductKitRepository : Repository<AppDataContext>, IProductKitRepository
    {
        public EfProductKitRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
        {
        }

        public async Task<ProductKit?> GetAsync(ProductKitId id)
        {
            var dbe = await Context.ProductKits.FirstOrDefaultAsync(p => p.Id == id.Guid);
            if (dbe == null) return null;

            return DbMapper.ToEntity<ProductKit>(dbe);
        }

        public async Task<ProductKitVersion?> GetVersionAsync(ProductKitVersionId id)
        {
            var dbe = await Context.ProductKitVersions
                .Include(p => p.ComponentMaps)
                .FirstOrDefaultAsync(p => p.Id == id.Guid);
            if (dbe == null) return null;

            return DbMapper.ToEntity<ProductKitVersion>(dbe);
        }

        public void Add(ProductKit productKit)
        {
            var dbe = DbMapper.ToDb<DbProductKit>(productKit);
            Context.ProductKits.Add(dbe);
        }

        public void AddVersion(ProductKitVersion version)
        {
            var dbe = DbMapper.ToDb<DbProductKitVersion>(version);
            Context.ProductKitVersions.Add(dbe);
        }
    }
}
