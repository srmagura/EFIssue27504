namespace DataInterfaces.Repositories
{
    public interface IComponentRepository
    {
        void Add(Component component);

        void AddVersion(ComponentVersion version);

        Task<Component?> GetAsync(ComponentId id);

        Task<ComponentVersion?> GetVersionAsync(ComponentVersionId id);

        Task<ComponentVersion[]> ListVersionsByIdAsync(ComponentVersionId[] ids);
    }
}
