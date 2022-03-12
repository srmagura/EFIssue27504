namespace DataInterfaces.Repositories
{
    public interface IProductFamilyRepository
    {
        void Add(ProductFamily productFamily);

        Task<ProductFamily?> GetAsync(ProductFamilyId id);
    }
}
