using DomainModel;

namespace Repository
{
    public interface IProductService : IDataService<Product>
    {
        Task<IEnumerable<Product>> GetByCategory(int categoryId, int count = 0);
        Task<IEnumerable<Product>> GetByBrand(int categoryId, int count = 0);
        Task<IEnumerable<Product>> Search(string text);
    }
}