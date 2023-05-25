using DomainModel;

namespace Repository
{
    public interface IProductImageService : IDataService<ProductImage>
    {
        Task<ProductImage?> GetMainByProductId(int productId);
        Task<IEnumerable<ProductImage?>> GetOtherByProductId(int productId);
    }
}