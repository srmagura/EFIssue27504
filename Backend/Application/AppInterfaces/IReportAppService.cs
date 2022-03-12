namespace AppInterfaces;

public interface IReportAppService
{
    Task<ReportDto?> GetAsync(ReportId id);
    Task<string?> GetPdfAsync(ReportId id, Stream outputStream); // Returns "application/pdf"

    Task<ReportId> RequestDraftAsync(ProjectId projectId, ReportType reportType);
}
