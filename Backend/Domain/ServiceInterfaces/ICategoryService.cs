using Entities;

namespace ServiceInterfaces
{
    public interface ICategoryService
    {
        void DiffCategories(List<Category> categories, List<Category> inputCategories);
    }
}
