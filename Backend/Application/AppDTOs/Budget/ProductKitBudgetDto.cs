namespace AppDTOs.Budget
{
    public class ProductKitBudgetDto
    {
        public ProductKitBudgetDto(ProductKitId productKitId, CategoryId categoryId, string name, int quantity, decimal sellPrice, int? length = null)
        {
            ProductKitId = productKitId;
            CategoryId = categoryId;
            Name = name;
            Quantity = quantity;
            SellPrice = sellPrice;
            Length = length;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public ProductKitId ProductKitId { get; set; }
        public CategoryId CategoryId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal SellPrice { get; set; }
        public int? Length { get; set; }
    }
}
