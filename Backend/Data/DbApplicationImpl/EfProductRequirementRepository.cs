namespace DbApplicationImpl;

public class EfProductRequirementRepository : Repository<AppDataContext>, IProductRequirementRepository
{
    public EfProductRequirementRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public void Add(ProductRequirement productRequirement)
    {
        var dbe = DbMapper.ToDb<DbProductRequirement>(productRequirement);
        Context.ProductRequirements.Add(dbe);
    }

    public async Task<ProductRequirement?> GetAsync(ProductRequirementId id)
    {
        var dbe = await Context.ProductRequirements.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<ProductRequirement>(dbe);
    }

    public async Task RemoveAsync(ProductRequirementId id)
    {
        var dbe = await Context.ProductRequirements.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return;

        Context.ProductRequirements.Remove(dbe);
    }

    public async Task SetIndexAsync(ProductRequirementId id, int index)
    {
        var dbe = await Context.ProductRequirements.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return;

        var all = await Context.ProductRequirements
            .Where(p => p.OrganizationId == dbe.OrganizationId)
            .OrderBy(p => p.Index)
            .ToListAsync();

        dbe = all.First(p => p.Id == id.Guid);
        all.Remove(dbe);
        all.Insert(index, dbe);

        var i = 0;
        foreach (var item in all)
        {
            item.Index = i;
            i++;
        }
    }
}
