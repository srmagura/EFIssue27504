namespace DataInterfaces.Queries;

public interface IImportQueries
{
    Task<ImportDto?> GetAsync(ImportId id);
    Task<ImportDto[]> ListAsync(ProjectId projectId, int skip, int take);
}
