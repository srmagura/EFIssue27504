using AppDTOs.Budget;
using AppDTOs.Designer;
using Entities;

namespace ServiceInterfaces
{
    public interface IBudgetService
    {
        decimal GetTotalCost(
            PlacedProductKitDto[] placedProductKits,
            ProductKit[] productKits,
            ProductKitVersion[] productKitVersions
        );

        ProjectBudgetDto CalculateBudget(
            Project project,
            PlacedProductKitDto[] placedProductKits,
            Category[] categories,
            ProductKit[] productKits,
            ProductKitVersion[] productKitVersions
        );
    }
}
