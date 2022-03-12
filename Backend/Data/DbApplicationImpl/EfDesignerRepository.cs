using ITI.Baseline.Util;

namespace DbApplicationImpl;

public class EfDesignerRepository : Repository<AppDataContext>, IDesignerRepository
{
    public EfDesignerRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task AddOrUpdateAsync(PageId pageId, DesignerDataType type, string json)
    {
        var exists = await Context.DesignerData.AnyAsync(d => d.PageId == pageId.Guid && d.Type == type);

        if (exists)
        {
            var data = await Context.DesignerData
                .FirstOrDefaultAsync(d => d.PageId == pageId.Guid && d.Type == type);

            Require.NotNull(data, "Designer data not found.");
            data.Json = json;
        }
        else
        {
            var organizationGuid = await Context.Pages
                .Where(p => p.Id == pageId.Guid)
                .Select(p => p.OrganizationId)
                .FirstAsync();

            Context.DesignerData.Add(
               new DbDesignerData(
                   organizationGuid,
                   pageId.Guid,
                   type,
                   json
               )
           );
        }
    }
}
