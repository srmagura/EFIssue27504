using Identities;

namespace InfraInterfaces;

public interface IReportProcessor
{
    Task ProcessAsync(ReportId reportId, CancellationToken cancellationToken);
}
