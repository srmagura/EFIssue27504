using Identities;
using ValueObjects;

namespace InfraInterfaces;

public interface IReportBuilder
{
    Task BuildAsync(
        ProjectId projectId,
        string filePath,
        Func<Percentage, Task> onProgressAsync,
        bool isDraft,
        bool devGenerateHtml,
        CancellationToken cancellationToken
    );
}
