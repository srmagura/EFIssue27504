namespace DbApplicationImpl;

public class EfReportRepository : Repository<AppDataContext>, IReportRepository
{
    public EfReportRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public async Task<Report?> GetAsync(ReportId id)
    {
        var dbe = await Context.Reports.FirstOrDefaultAsync(i => i.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<Report>(dbe);
    }

    public void Add(Report report)
    {
        var dbe = DbMapper.ToDb<DbReport>(report);
        Context.Add(dbe);
    }
}
