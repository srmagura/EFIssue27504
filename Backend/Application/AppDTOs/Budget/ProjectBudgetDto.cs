namespace AppDTOs.Budget
{
    public class ProjectBudgetDto
    {
        public ProjectBudgetDto(
            ProjectId projectId,
            decimal totalCost,
            decimal deposit,
            decimal pricePerSquareFoot,
            List<CategoryBudgetDto> categories
        )
        {
            ProjectId = projectId;
            TotalCost = totalCost;
            Deposit = deposit;
            PricePerSquareFoot = pricePerSquareFoot;
            Categories = categories;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public ProjectId ProjectId { get; set; }

        public decimal TotalCost { get; set; }
        public decimal Deposit { get; set; }
        public decimal PricePerSquareFoot { get; set; }

        public List<CategoryBudgetDto> Categories { get; set; }
    }
}
