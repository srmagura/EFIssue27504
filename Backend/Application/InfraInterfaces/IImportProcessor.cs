using Identities;

namespace InfraInterfaces;

public interface IImportProcessor
{
    Task ProcessAsync(ImportId importId, CancellationToken cancellationToken);
}
