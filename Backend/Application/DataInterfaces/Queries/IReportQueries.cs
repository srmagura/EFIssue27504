using Enumerations;

namespace DataInterfaces.Queries;

public interface IReportQueries
{
    Task<ReportDto?> GetAsync(ReportId id);
    Task<ReportSystemDto?> GetSystemAsync(ReportId id);

    Task<bool> AnyAsync(ProjectPublicationId projectPublicationId, ReportType type);
}
