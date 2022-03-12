using AppDTOs.Designer;
using System.Text.Json;

namespace DbApplicationImpl;

public class EfProductKitReferenceRepository : Repository<AppDataContext>, IProductKitReferenceRepository
{
    public EfProductKitReferenceRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<ProductKitReference?> GetAsync(ProductKitReferenceId id)
    {
        var dbe = await Context.ProductKitReferences.FirstOrDefaultAsync(r => r.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<ProductKitReference>(dbe);
    }

    public async Task<(ProductKitReference Reference, ProductKitVersionId LatestVersionId)[]> ListWithLatestVersionAsync(ProjectId projectId)
    {
        var referenceData = await Context.ProductKitReferences
            .Select(r => new
            {
                Reference = r,
                r.ProductKitVersion!.ProductKitId
            })
            .ToArrayAsync();

        var productKitIds = referenceData
            .Select(d => d.ProductKitId)
            .ToArray();

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

        var latestVersionIdDictionary = productKitsWithLatestVersion
            .ToDictionary(
                p => p.Id,
                p => new ProductKitVersionId(p.Versions[0].Id)
            );

        return referenceData
            .Select(d =>
            {
                var found = latestVersionIdDictionary.TryGetValue(d.ProductKitId, out var latestVersionId);
                if (!found)
                {
                    throw new Exception($"Could not determine latest version for product kit {d.ProductKitId}.");
                }

                var referenceDomainEntity = DbMapper.ToEntity<ProductKitReference>(d.Reference);

                return (referenceDomainEntity, latestVersionId!);
            })
            .ToArray();
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private async Task<List<ProductKitId>> ListProductKitIdsAsync(ProjectId projectId)
    {
        // This includes inactive pages
        var jsonArray = await Context.DesignerData
            .Where(d =>
                d.Page!.ProjectId == projectId.Guid &&
                d.Type == DesignerDataType.PlacedProductKits
            )
            .Select(d => d.Json)
            .ToArrayAsync();

        var productKitIds = new List<ProductKitId>();

        foreach (var json in jsonArray)
        {
            var placedProductKits = JsonSerializer.Deserialize<PlacedProductKitDto[]>(json, JsonSerializerOptions);
            if (placedProductKits == null) continue;

            productKitIds.AddRange(placedProductKits.Select(p => p.ProductKitId));
        }

        return productKitIds;
    }

    public async Task AddAndRemoveReferencesAsync(OrganizationId organizationId, ProjectId projectId)
    {
        var productKitIds = await ListProductKitIdsAsync(projectId);

        var referenceData = await Context.ProductKitReferences
            .Where(r => r.ProjectId == projectId.Guid)
            .Select(r => new
            {
                ProductKitReference = r,
                ProductKitId = new ProductKitId(r.ProductKitVersion!.ProductKitId)
            })
            .ToArrayAsync();

        var dbProductKitIds = referenceData.Select(r => r.ProductKitId).ToHashSet();

        // Add
        foreach (var productKitId in productKitIds)
        {
            if (!dbProductKitIds.Contains(productKitId))
            {
                var latestVersionId = await Context.ProductKitVersions
                    .Where(v => v.OrganizationId == organizationId.Guid && v.ProductKitId == productKitId.Guid)
                    .OrderByDescending(v => v.DateCreatedUtc)
                    .Select(v => v.Id)
                    .FirstAsync();

                Context.ProductKitReferences.Add(
                    new DbProductKitReference(organizationId.Guid, projectId.Guid, latestVersionId)
                );
            }
        }

        // Remove
        foreach (var data in referenceData)
        {
            if (!productKitIds.Contains(data.ProductKitId))
            {
                Context.ProductKitReferences.Remove(data.ProductKitReference);
            }
        }
    }
}
