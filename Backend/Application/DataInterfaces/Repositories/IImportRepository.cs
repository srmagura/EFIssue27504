namespace DataInterfaces.Repositories;

public interface IImportRepository
{
    Task<Import?> GetAsync(ImportId id);

    void Add(Import import);
}
