using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DbApplicationImpl;

public class EfProductFamilyQueries : Queries<AppDataContext>, IProductFamilyQueries
{
    public EfProductFamilyQueries(IUnitOfWorkProvider uowp) : base(uowp)
    {
    }

    public async Task<FilteredList<ProductFamilyDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    )
    {
        var q = Context.ProductFamilies
            .Include(p => p.ProductKits)
            .ThenInclude(p => p.Versions
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(1)
            )
            .Where(p => p.OrganizationId == organizationId.Guid);

        switch (activeFilter)
        {
            case ActiveFilter.ActiveOnly:
                q = q.Where(p => p.IsActive);
                break;
            case ActiveFilter.InactiveOnly:
                q = q.Where(p => !p.IsActive);
                break;
            case ActiveFilter.All:
                break;
        }

        if (search.HasValue())
        {
            q = q.Where(p => p.Name.Contains(search));
        }

        var count = await q.CountAsync();

        q = q.OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take);

        var items = (await q.ToArrayAsync())
            .Select(p =>
            {
                var productKits = p.ProductKits.Select(pp =>
                {
                    var v = pp.Versions[0];

                    return new ProductKitReferenceDto(
                        id: new ProductKitId(pp.Id),
                        name: v.Name,
                        sellPrice: v.SellPrice.Value
                    );
                })
                .OrderByDescending(p => p.SellPrice)
                .ToArray();

                return new ProductFamilyDto(
                    new ProductFamilyId(p.Id),
                    p.Name,
                    p.IsActive,
                    productKits
               );
            })
            .ToArray();

        return new FilteredList<ProductFamilyDto>(items, count);
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        return Context.ProductFamilies
            .Where(p => p.OrganizationId == organizationId.Guid)
            .AllAsync(p => p.Name != name);
    }
}
