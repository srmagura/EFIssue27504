namespace AppInterfaces;

public interface IImportAppService
{
    Task<ImportDto[]> ListAsync(ProjectId projectId, int skip, int take);

    Task<ImportId> ImportAsync(ProjectId projectId, string filename, Stream stream);
}
