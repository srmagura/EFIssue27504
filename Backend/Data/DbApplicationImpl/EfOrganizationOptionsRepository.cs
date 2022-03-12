namespace DbApplicationImpl;

public class EfOrganizationOptionsRepository : Repository<AppDataContext>, IOrganizationOptionsRepository
{
    public EfOrganizationOptionsRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<DefaultProjectDescription?> GetDefaultProjectDescriptionAsync(OrganizationId organizationId)
    {
        var dbe = await Context.DefaultProjectDescriptions.FirstOrDefaultAsync(p => p.OrganizationId == organizationId.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<DefaultProjectDescription>(dbe);
    }

    public async Task<NotForConstructionDisclaimer?> GetNotForConstructionDisclaimerAsync(OrganizationId organizationId)
    {
        var dbe = await Context.NotForConstructionDisclaimers.FirstOrDefaultAsync(p => p.OrganizationId == organizationId.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<NotForConstructionDisclaimer>(dbe);
    }

    public void Add(DefaultProjectDescription defaultProjectDescription)
    {
        var dbe = DbMapper.ToDb<DbDefaultProjectDescription>(defaultProjectDescription);
        Context.DefaultProjectDescriptions.Add(dbe);
    }

    public void Add(NotForConstructionDisclaimer disclaimer)
    {
        var dbe = DbMapper.ToDb<DbNotForConstructionDisclaimer>(disclaimer);
        Context.NotForConstructionDisclaimers.Add(dbe);
    }
}
