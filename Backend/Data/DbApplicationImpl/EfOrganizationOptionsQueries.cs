namespace DbApplicationImpl;

public class EfOrganizationOptionsQueries : Queries<AppDataContext>, IOrganizationOptionsQueries
{
    public EfOrganizationOptionsQueries(IUnitOfWorkProvider uowp) : base(uowp)
    {
    }

    public async Task<string?> GetDefaultProjectDescriptionAsync(OrganizationId organizationId)
    {
        var description = await Context.DefaultProjectDescriptions
            .Where(p => p.OrganizationId == organizationId.Guid)
            .FirstOrDefaultAsync();

        return description?.Text;
    }

    public async Task<string?> GetNotForConstructionDisclaimerTextAsync(OrganizationId organizationId)
    {
        var disclaimer = await Context.NotForConstructionDisclaimers
            .Where(p => p.OrganizationId == organizationId.Guid)
            .FirstOrDefaultAsync();

        return disclaimer?.Text;
    }
}
