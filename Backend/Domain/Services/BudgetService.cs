using AppDTOs;
using AppDTOs.Budget;
using AppDTOs.Designer;
using Entities;
using Enumerations;
using Identities;
using ITI.Baseline.Util;
using ServiceInterfaces;

namespace Services
{
    public class BudgetService : IBudgetService
    {
        public BudgetService() { }

        private decimal CalculateTotalCost(PlacedProductKitDto[] placedProductKits, ProductKit[] productKits, ProductKitVersion[] productKitVersions)
        {
            decimal totalCost = 0;

            foreach (var placedProductKit in placedProductKits)
            {
                var productKit = productKits.First(p => p.Id == placedProductKit.ProductKitId);
                var productKitVersion = productKitVersions.First(p => p.ProductKitId == placedProductKit.ProductKitId);

                if (productKit.MeasurementType == MeasurementType.Linear)
                {
                    Require.NotNull(placedProductKit.LengthInches, "A placed product kit has measurement type linear but the length is null.");
                    totalCost += productKitVersion.SellPrice.Value * placedProductKit.LengthInches.Value;
                }
                else
                {
                    totalCost += productKitVersion.SellPrice.Value;
                }
            }

            return totalCost;
        }

        private List<CategoryBudgetDto> GetCategoryBudgets(
            PlacedProductKitDto[] placedProductKits,
            Category[] categories,
            ProductKit[] productKits,
            ProductKitVersion[] productKitVersions)
        {
            // Length is unique as well
            var distinctProductKits = placedProductKits.Select(p => new { Id = p.ProductKitId, Length = p.LengthInches }).Distinct();
            var productKitBudgets = new List<ProductKitBudgetDto>();
            var usedCategories = new Dictionary<CategoryId, Category>();
            foreach (var distinctProductKit in distinctProductKits)
            {
                var quantity = placedProductKits.Count(p => p.ProductKitId == distinctProductKit.Id && p.LengthInches == distinctProductKit.Length);
                var productKit = productKits.First(p => p.Id == distinctProductKit.Id);
                var categoryId = productKit.CategoryId;
                var productKitVersion = productKitVersions.First(p => p.ProductKitId == distinctProductKit.Id);

                productKitBudgets.Add(new ProductKitBudgetDto(
                    distinctProductKit.Id,
                    categoryId,
                    productKitVersion.Name,
                    quantity,
                    productKitVersion.SellPrice.Value,
                    distinctProductKit.Length
                ));

                if (!usedCategories.ContainsKey(categoryId))
                {
                    usedCategories.Add(categoryId, categories.First(p => p.Id == categoryId));
                }
            }

            var categoryBudgets = new List<CategoryBudgetDto>();

            foreach (var (id, category) in usedCategories)
            {
                var categoryPKBudgets = productKitBudgets.Where(p => p.CategoryId == id).ToList();
                categoryBudgets.Add(new CategoryBudgetDto(
                    id,
                    category.Name,
                    category.Color!,
                    categoryPKBudgets
                ));
            }

            return categoryBudgets;
        }

        // TODO:AQ Use this in the future ProjectBudgetProcessor
        public decimal GetTotalCost(
            PlacedProductKitDto[] placedProductKits,
            ProductKit[] productKits,
            ProductKitVersion[] productKitVersions
        )
        {
            return CalculateTotalCost(placedProductKits, productKits, productKitVersions);
        }

        public ProjectBudgetDto CalculateBudget(
            Project project,
            PlacedProductKitDto[] placedProductKits,
            Category[] categories,
            ProductKit[] productKits,
            ProductKitVersion[] productKitVersions
        )
        {
            var totalCost = CalculateTotalCost(placedProductKits, productKits, productKitVersions);
            var deposit = project.BudgetOptions.DepositPercentage.Value * totalCost;
            var pricePerSquareFoot = totalCost / project.EstimatedSquareFeet;
            var categoryBudgets = GetCategoryBudgets(placedProductKits, categories, productKits, productKitVersions);

            return new ProjectBudgetDto(
                project.Id,
                totalCost,
                deposit,
                pricePerSquareFoot,
                categoryBudgets
            );
        }
    }
}
