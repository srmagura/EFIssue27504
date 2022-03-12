namespace AppDTOs.Budget
{
    public class CategoryBudgetDto
    {
        public CategoryBudgetDto(
            CategoryId categoryId,
            string name,
            string color,
            List<ProductKitBudgetDto> productKits
        )
        {
            CategoryId = categoryId;
            Name = name;
            Color = color;
            ProductKits = productKits;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public CategoryId CategoryId { get; set; }

        public string Name { get; set; }
        public string Color { get; set; }

        public List<ProductKitBudgetDto> ProductKits { get; set; }
    }
}
