namespace DbApplicationImpl;

public class EfAppPermissionsQueries : Queries<AppDataContext>, IAppPermissionsQueries
{
    public EfAppPermissionsQueries(IUnitOfWorkProvider uowp) : base(uowp)
    {
    }

    // All of these queries should bypass row-level security since:
    // - The UI can request permissions for multiple organizations in a single API call, and
    // - These queries don't return any sensitive data

    public async Task<OrganizationId> OrganizationOfAsync(UserId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.Users
            .Where(p => p.Id == id.Guid)
            .Select(p => new OrganizationId(p.OrganizationId))
            .FirstAsync();
    }

    public async Task<OrganizationId> OrganizationOfAsync(ProjectId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.Projects
            .Where(p => p.Id == id.Guid)
            .Select(p => new OrganizationId(p.OrganizationId))
            .FirstAsync();
    }

    public async Task<OrganizationId> OrganizationOfAsync(ComponentId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.Components
            .Where(p => p.Id == id.Guid)
            .Select(p => new OrganizationId(p.OrganizationId))
            .FirstAsync();
    }

    public async Task<OrganizationId> OrganizationOfAsync(ComponentVersionId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.ComponentVersions
            .Where(p => p.Id == id.Guid)
            .Select(p => new OrganizationId(p.OrganizationId))
            .FirstAsync();
    }

    public async Task<OrganizationId> OrganizationOfAsync(ProductKitId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.ProductKits
            .Where(p => p.Id == id.Guid)
            .Select(p => new OrganizationId(p.OrganizationId))
            .FirstAsync();
    }

    public async Task<OrganizationId> OrganizationOfAsync(PageId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.Pages
            .Where(p => p.Id == id.Guid)
            .Select(p => new OrganizationId(p.OrganizationId))
            .FirstAsync();
    }

    public async Task<OrganizationId> OrganizationOfAsync(ProductRequirementId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.ProductRequirements
            .Where(p => p.Id == id.Guid)
            .Select(p => new OrganizationId(p.OrganizationId))
            .FirstAsync();
    }

    public async Task<UserRole> UserRoleForAsync(UserId id)
    {
        using var _ = new SystemSecurityScope();

        return await Context.Users
            .Where(p => p.Id == id.Guid)
            .Select(p => p.Role)
            .FirstAsync();
    }
}
