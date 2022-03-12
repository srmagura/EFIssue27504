using DataInterfaces.Repositories;
using Entities;
using ITI.Baseline.Util;
using ServiceInterfaces;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public void DiffCategories(List<Category> categories, List<Category> inputCategories)
        {
            // Add/update
            foreach (var inputCategory in inputCategories)
            {
                var existingCategory = categories.FirstOrDefault(c => c.Id == inputCategory.Id);

                if (existingCategory == null)
                {
                    _repo.Add(inputCategory);
                }
                else
                {
                    Category? inputParentCategory = null;

                    if (inputCategory.ParentId != null)
                    {
                        inputParentCategory = inputCategories.FirstOrDefault(c => c.Id == inputCategory.ParentId);
                        Require.NotNull(inputParentCategory, "Parent category not found.");
                    }

                    existingCategory.UpdateFrom(inputCategory, inputParentCategory);
                }
            }
        }
    }
}
