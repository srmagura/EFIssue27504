namespace TestDataLoader.Helpers;

internal static class LoaderQueries
{
    internal static Task<ProjectId> GetFirstHostOrganizationProjectAsync(AppDataContext db)
    {
        return db.Projects
            .Where(p => p.Organization!.IsHost && p.IsActive)
            .OrderBy(p => p.Name)
            .Select(p => new ProjectId(p.Id))
            .FirstAsync();
    }

    internal static Task<OrganizationId> GetHostOrganizationAsync(AppDataContext db)
    {
        return db.Organizations
            .Where(p => p.IsHost)
            .Select(p => new OrganizationId(p.Id))
            .FirstAsync();
    }
}
