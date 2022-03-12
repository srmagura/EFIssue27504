namespace DbApplicationImpl
{
    public class EfCategoryRepository : Repository<AppDataContext>, ICategoryRepository
    {
        public EfCategoryRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
            : base(uowp, dbMapper)
        {
        }

        public async Task<List<Category>> AllForAsync(OrganizationId organizationId)
        {
            var dbes = await Context.Categories
                   .Where(p => p.OrganizationId == organizationId.Guid)
                   .ToListAsync();

            return dbes
                .Select(p => DbMapper.ToEntity<Category>(p))
                .ToList();
        }

        public async Task<Category?> GetAsync(CategoryId id)
        {
            var dbe = await Context.Categories.FirstOrDefaultAsync(p => p.Id == id.Guid);
            if (dbe == null) return null;

            return DbMapper.ToEntity<Category>(dbe);
        }

        public void Add(Category category)
        {
            var dbe = DbMapper.ToDb<DbCategory>(category);
            Context.Categories.Add(dbe);
        }
    }
}
