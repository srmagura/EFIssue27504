using AppDTOs.Enumerations;

namespace DbApplicationImpl;

public class EfCategoryQueries : Queries<AppDataContext>, ICategoryQueries
{
    public EfCategoryQueries(IUnitOfWorkProvider uowp) : base(uowp)
    {
    }

    public async Task<DbCategory[]> ListDbAsync(OrganizationId organizationId, ActiveFilter activeFilter)
    {
        var q = Context.Categories.Where(p => p.OrganizationId == organizationId.Guid);

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

        return await q
            .OrderBy(p => p.Index)
            .AsNoTracking()
            .ToArrayAsync();
    }
}
