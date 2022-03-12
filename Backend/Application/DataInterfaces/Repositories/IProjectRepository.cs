namespace DataInterfaces.Repositories
{
    public interface IProjectRepository
    {
        void Add(Project project);
        Task<Project?> GetAsync(ProjectId id);
    }
}
