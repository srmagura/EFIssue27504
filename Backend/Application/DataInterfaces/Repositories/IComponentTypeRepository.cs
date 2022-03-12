namespace DataInterfaces.Repositories
{
    public interface IComponentTypeRepository
    {
        void Add(ComponentType componentType);

        Task<ComponentType?> GetAsync(ComponentTypeId componentTypeId);
    }
}
