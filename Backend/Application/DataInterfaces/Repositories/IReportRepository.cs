namespace DataInterfaces.Repositories;

public interface IReportRepository
{
    Task<Report?> GetAsync(ReportId id);

    void Add(Report report);
}
