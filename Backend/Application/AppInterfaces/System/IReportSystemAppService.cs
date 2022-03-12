namespace AppInterfaces.System;

public interface IReportSystemAppService
{
    Task<ReportSystemDto?> GetAsync(ReportId id);

    Task RequestAsync(
        ProjectId projectId,
        ProjectPublicationId? projectPublicationId,
        ReportType type
    );

    Task BeginProcessingAsync(ReportId id);
    Task SetPercentCompleteAsync(ReportId id, decimal percentComplete);

    Task SetCompletedAsync(ReportId id, FileId fileId, string fileType);
    Task SetErrorAsync(ReportId id, string errorMessage);
    Task SetCanceledAsync(ReportId id);
}
