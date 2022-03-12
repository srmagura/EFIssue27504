namespace DataInterfaces.Repositories
{
    public interface IProductPhotoRepository
    {
        void Add(ProductPhoto productPhoto);
        Task<ProductPhoto?> GetAsync(ProductPhotoId id);
    }
}
