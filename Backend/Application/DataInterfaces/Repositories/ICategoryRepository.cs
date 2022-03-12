namespace DataInterfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetAsync(CategoryId id);
        Task<List<Category>> AllForAsync(OrganizationId organizationId);

        void Add(Category category);
    }
}
