using ValueObjects;

namespace AppDTOs
{
    public class ProjectBudgetOptionsDto
    {
        public ProjectBudgetOptionsDto() { }

        public ProjectBudgetOptionsDto(
            decimal costAdjustment,
            decimal depositPercentage,
            bool showPricingInBudgetBreakdown,
            bool showPricePerSquareFoot
        )
        {
            CostAdjustment = costAdjustment;
            DepositPercentage = depositPercentage;
            ShowPricingInBudgetBreakdown = showPricingInBudgetBreakdown;
            ShowPricePerSquareFoot = showPricePerSquareFoot;
        }

        public decimal CostAdjustment { get; set; }
        public decimal DepositPercentage { get; set; }

        public bool ShowPricingInBudgetBreakdown { get; set; }
        public bool ShowPricePerSquareFoot { get; set; }

        public ProjectBudgetOptions ToValueObject()
        {
            return new ProjectBudgetOptions(
                new Percentage(CostAdjustment),
                new Percentage(DepositPercentage),
                ShowPricingInBudgetBreakdown,
                ShowPricePerSquareFoot
            );
        }
    }
}
