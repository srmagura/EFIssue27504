namespace DbApplicationImpl;

public class EfProductKitReferenceQueries : Queries<AppDataContext>, IProductKitReferenceQueries
{
    public EfProductKitReferenceQueries(IUnitOfWorkProvider uowp) : base(uowp)
    {
    }

    public async Task<ProductKitReferenceSummaryDto[]> ListAsync(ProjectId projectId)
    {
        var referenceData = await Context.ProductKitReferences
            .AsNoTracking()
            .Where(r => r.ProjectId == projectId.Guid)
            .OrderBy(r => r.ProductKitVersion!.Name)
            .Select(r => new
            {
                Id = new ProductKitReferenceId(r.Id),
                ProductKitVersionId = new ProductKitVersionId(r.ProductKitVersionId),
                r.Tag,
                r.ProductKitVersion!.ProductKitId,
                r.ProductKitVersion.VersionName,
                r.ProductKitVersion.SellPrice,
                ProductKitName = r.ProductKitVersion.Name,
                MainComponentName = r.ProductKitVersion.MainComponentVersion!.DisplayName,
            })
            .ToArrayAsync();

        var productKitIds = referenceData.Select(r => r.ProductKitId).ToArray();

        // Inefficient but simplest way to do it, optimize if it becomes a problem
        var productKitsWithLatestVersion = await Context.ProductKits
            .AsNoTracking()
            .Include(p => p.Versions
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(1)
            )
            .Where(p => productKitIds.Contains(p.Id))
            // Must bring back from the database before doing a select for the filtered
            // include to work correctly
            .ToArrayAsync();

        var latestVersionDictionary = productKitsWithLatestVersion
            .ToDictionary(
                p => p.Id,
                p => new
                {
                    ProductKitVersionId = new ProductKitVersionId(p.Versions[0].Id),
                    p.Versions[0].VersionName,
                }
            );

        return referenceData
            .Select(r =>
            {
                var found = latestVersionDictionary.TryGetValue(r.ProductKitId, out var latestVersion);
                if (!found) return null;

                return new ProductKitReferenceSummaryDto(
                    id: r.Id,
                    productKitId: new ProductKitId(r.ProductKitId),
                    productKitVersionId: r.ProductKitVersionId,
                    versionName: r.VersionName,
                    productKitName: r.ProductKitName,
                    mainComponentName: r.MainComponentName,
                    latestProductKitVersionId: latestVersion!.ProductKitVersionId,
                    latestVersionName: latestVersion.VersionName
                )
                {
                    SellPrice = r.SellPrice.Value,
                    Tag = r.Tag
                };
            })
            .Where(r => r != null)
            .Select(r => r!)
            .ToArray();
    }
}
