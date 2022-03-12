namespace DbApplicationImpl;

public class EfReportQueries : Queries<AppDataContext>, IReportQueries
{
    private readonly IMapper _mapper;

    public EfReportQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<ReportDto?> GetAsync(ReportId id)
    {
        return Context.Reports
            .Where(r => r.Id == id.Guid)
            .ProjectToDtoAsync<DbReport, ReportDto>(_mapper);
    }

    public Task<ReportSystemDto?> GetSystemAsync(ReportId id)
    {
        return Context.Reports
            .Where(r => r.Id == id.Guid)
            .ProjectToDtoAsync<DbReport, ReportSystemDto>(_mapper);
    }

    public Task<bool> AnyAsync(ProjectPublicationId projectPublicationId, ReportType type)
    {
        return Context.Reports.AnyAsync(r =>
            r.ProjectPublicationId == projectPublicationId.Guid &&
            r.Type == type
        );
    }
}
