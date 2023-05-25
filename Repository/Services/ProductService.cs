using DomainModel;
using Framework;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductService : IProductService, IRegisterScoped
    {
        private readonly ShopContext _ShopContext;
        public ProductService(ShopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task Add(Product entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.EditedBy = null;
            entity.CreatedAt = entity.CreatedAt;
            entity.DeletedBy = null;
            entity.DeletedAt = null;
            entity.IsDelete = false;
            await _ShopContext.AddAsync(entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Update(Product Entity)
        {
            Entity.EditedAt = DateTime.Now;
            Entity.DeletedBy = null;
            Entity.DeletedAt = null;
            Entity.IsDelete = false;
            _ShopContext.Update(Entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Delete(Product entity)
        {
            entity.DeletedAt = DateTime.Now;
            entity.IsDelete = true;
            await _ShopContext.SaveChangesAsync();
        }

        public async Task<Product?> Get(int id)
        {
            var _Category = new Product();
            if (id.IsNotZero())
            {
                _Category = await _ShopContext.Products.FindAsync(id);
                if (_Category != null && !_Category.IsDelete)
                    return _Category;
            }
            return _Category;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _ShopContext.Products.Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByUser(int userId)
        {
            return await _ShopContext.Products.Where(x => !x.IsDelete && x.CreatedBy == userId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategory(int categoryId, int count = 0)
        {
            var models = await _ShopContext.Products.Where(x => !x.IsDelete && x.CategoryId == categoryId).If(count.IsNotZero(), x => x.Take(count)).ToListAsync();
            return models;
        }

        public async Task<IEnumerable<Product>> GetByBrand(int brandId, int count = 0)
        {
            return await _ShopContext.Products.Where(x => !x.IsDelete && x.BrandId == brandId).If(count.IsNotZero(), x => x.Take(count)).ToListAsync();
        }

        public async Task<bool> Any(int id)
        {
            return await Get(id) != null;
        }

        public async Task<IEnumerable<Product>> Search(string text)
        {
            return await _ShopContext.Products.Where(x => !x.IsDelete && (text.IsEmpty() || x.Description.Contains(text) || x.Title.Contains(text))).ToListAsync();
        }
    }
}