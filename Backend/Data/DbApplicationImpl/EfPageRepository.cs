namespace DbApplicationImpl;

public class EfPageRepository : Repository<AppDataContext>, IPageRepository
{
    public EfPageRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public void Add(Page page)
    {
        var dbe = DbMapper.ToDb<DbPage>(page);
        Context.Pages.Add(dbe);
    }

    public void Add(List<Page> pages)
    {
        foreach (var page in pages)
        {
            Add(page);
        }
    }

    public async Task<Page?> GetAsync(PageId id)
    {
        var dbe = await Context.Pages.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<Page>(dbe);
    }

    public async Task RemoveAsync(PageId id)
    {
        var dbe = await Context.Pages
            .FirstOrDefaultAsync(p => p.Id == id.Guid);

        if (dbe == null)
            return;

        Context.Pages.Remove(dbe);
    }

    public async Task SetIndexAsync(PageId id, bool decreaseIndex)
    {
        var dbe = await Context.Pages.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null || !dbe.IsActive)
            return;

        var active = await Context.Pages
            .Where(p => p.ProjectId == dbe.ProjectId && p.IsActive)
            .OrderBy(p => p.Index)
            .ToListAsync();

        dbe = active.First(p => p.Id == id.Guid);
        var index = decreaseIndex ? dbe.Index - 1 : dbe.Index + 1;

        active.Remove(dbe);

        if (index <= 0)
            active.Insert(0, dbe);
        else if (index >= active.Count)
            active.Add(dbe);
        else
            active.Insert(index, dbe);

        var i = 0;
        foreach (var item in active)
        {
            item.Index = i;
            i++;
        }
    }

    public async Task SetIndexBeforeSetActiveAsync(PageId id, bool isActive)
    {
        var dbe = await Context.Pages.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null)
            return;

        var all = await Context.Pages
            .Where(p => p.ProjectId == dbe.ProjectId)
            .OrderBy(p => p.Index)
            .ToListAsync();

        dbe = all.First(p => p.Id == id.Guid);
        all.Remove(dbe);

        if (isActive)
            all.Add(dbe);
        else
            all.Insert(0, dbe);

        var i = 0;
        foreach (var item in all)
        {
            var itemIsActive = item.Id == id.Guid ? isActive : item.IsActive;

            if (itemIsActive)
            {
                item.Index = i++;
            }
            else
            {
                item.Index = 0;
            }
        }
    }

    public async Task IncrementIndicesAsync(ProjectId projectId, int beginIndex)
    {
        var toIncrement = await Context.Pages
            .Where(p => p.ProjectId == projectId.Guid && p.Index >= beginIndex)
            .OrderBy(p => p.Index)
            .ToListAsync();

        foreach (var page in toIncrement)
        {
            page.Index++;
        }
    }
}
