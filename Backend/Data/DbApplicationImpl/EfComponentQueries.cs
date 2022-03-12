using DbApplicationImpl.Util;
using ITI.Baseline.Util;

namespace DbApplicationImpl;

public class EfComponentQueries : Queries<AppDataContext>, IComponentQueries
{
    private readonly IMapper _mapper;

    public EfComponentQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public async Task<ComponentDto?> GetAsync(ComponentId id)
    {
        var q = Context.Components
            .Include(p => p.Versions)
            .Where(p => p.Id == id.Guid);

        var dto = await _mapper.ProjectToDtoAsync<DbComponent, ComponentDto>(q);
        if (dto == null) return null;

        dto.Versions = dto.Versions.OrderByDescending(v => v.DateCreatedUtc).ToList();
        return dto;
    }

    // Inefficient but simplest way to do it, optimize if it becomes a problem
    public async Task<ComponentSummaryDto[]> ListAsync(OrganizationId organizationId)
    {
        var components = await Context.Components
            .AsNoTracking()
            .Include(p => p.Versions
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(1)
            )
            .Include(p => p.ComponentType)
            .Where(p => p.OrganizationId == organizationId.Guid)
            .ToArrayAsync();

        return components
            .Where(p => p.Versions.Count == 1)
            .OrderBy(c => c.Versions[0].DisplayName.ToLowerInvariant())
            .Select(p => new ComponentSummaryDto(
                new ComponentId(p.Id),
                p.MeasurementType,
                p.IsVideoDisplay,
                p.IsActive,
                p.VisibleToCustomer,
                new ComponentVersionId(p.Versions[0].Id),
                p.Versions[0].DisplayName,
                p.Versions[0].VersionName,
                p.Versions[0].SellPrice.Value,
                p.ComponentType!.Name,
                p.Versions[0].Make,
                p.Versions[0].Model,
                p.Versions[0].VendorPartNumber,
                p.Versions[0].OrganizationPartNumber
            ))
            .ToArray();
    }

    public async Task<ComponentSummaryDto[]> ListComponentsByVersionId(List<ComponentVersionId> componentVersionIds)
    {
        var q = Context.Components
            .Include(p => p.ComponentType)
            .Include(p => p.Versions
                .Where(q => componentVersionIds.Select(r => r.Guid).Contains(q.Id))
                .Take(1)
            );

        return (await q.ToArrayAsync())
            .Where(p => p.Versions.Count == 1)
            .Select(p => new ComponentSummaryDto(
                new ComponentId(p.Id),
                p.MeasurementType,
                p.IsVideoDisplay,
                p.IsActive,
                p.VisibleToCustomer,
                new ComponentVersionId(p.Versions[0].Id),
                p.Versions[0].DisplayName,
                p.Versions[0].VersionName,
                p.Versions[0].SellPrice.Value,
                p.ComponentType!.Name,
                p.Versions[0].Make,
                p.Versions[0].Model,
                p.Versions[0].VendorPartNumber,
                p.Versions[0].OrganizationPartNumber
            ))
            .ToArray();
    }

    public async Task<string> GetNewVersionNameAsync(ComponentId id)
    {
        var nowName = DateTime.UtcNow.ToString("MMMyy");

        var versionNames = await Context.ComponentVersions
            .Where(p => p.ComponentId == id.Guid && p.VersionName.StartsWith(nowName))
            .Select(p => p.VersionName)
            .ToArrayAsync();

        return VersionNameUtil.GetNewVersionName(versionNames, nowName);
    }

    public async Task<bool> VersionsAreForDistinctComponentsAsync(List<ComponentVersionId> versionIds)
    {
        var guids = versionIds.Select(p => p.Guid);

        var componentIds = await Context.ComponentVersions
            .Where(p => guids.Contains(p.Id))
            .Select(p => p.ComponentId)
            .ToArrayAsync();

        return componentIds.Distinct().Count() == componentIds.Length;
    }

    public Task<bool> VendorPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string vendorPartNumber)
    {
        var q = Context.ComponentVersions.Where(p => p.OrganizationId == organizationId.Guid);

        if (componentId != null)
        {
            q = q.Where(p => p.ComponentId != componentId.Guid);
        }

        return q.AllAsync(p => p.VendorPartNumber != vendorPartNumber);
    }

    public Task<bool> OrganizationPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string organizationPartNumber)
    {
        var q = Context.ComponentVersions.Where(p => p.OrganizationId == organizationId.Guid);

        if (componentId != null)
        {
            q = q.Where(p => p.ComponentId != componentId.Guid);
        }

        return q.AllAsync(p => p.OrganizationPartNumber != organizationPartNumber);
    }
}
