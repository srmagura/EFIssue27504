namespace DataInterfaces.Repositories;

public interface IProjectPublicationRepository
{
    Task<ProjectPublication?> GetAsync(ProjectPublicationId id);

    void Add(ProjectPublication projectPublication);
}
