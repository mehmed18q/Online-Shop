using DomainModel;
using Framework;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductImageService : IProductImageService, IRegisterScoped
    {
        private readonly ShopContext _ShopContext;
        public ProductImageService(ShopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task Add(ProductImage entity)
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

        public async Task Update(ProductImage Entity)
        {
            Entity.EditedAt = DateTime.Now;
            Entity.DeletedBy = null;
            Entity.DeletedAt = null;
            Entity.IsDelete = false;
            _ShopContext.Update(Entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Delete(ProductImage entity)
        {
            entity.DeletedAt = DateTime.Now;
            entity.IsDelete = true;
            await _ShopContext.SaveChangesAsync();
        }

        public async Task<ProductImage?> Get(int id)
        {
            var _ProductImage = new ProductImage();
            if (id.IsNotZero())
            {
                _ProductImage = await _ShopContext.ProductImages.FindAsync(id);
                if (_ProductImage != null && !_ProductImage.IsDelete)
                    return _ProductImage;
            }
            return _ProductImage;
        }

        public async Task<IEnumerable<ProductImage>> GetAll()
        {
            return await _ShopContext.ProductImages.Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetByUser(int userId)
        {
            return await _ShopContext.ProductImages.Where(x => !x.IsDelete && x.CreatedBy == userId).ToListAsync();
        }

        public async Task<ProductImage?> GetMainByProductId(int productId)
        {
            return await _ShopContext.ProductImages.FirstOrDefaultAsync(x => !x.IsDelete && x.IsMain && x.ProductId == productId);
        }

        public async Task<IEnumerable<ProductImage?>> GetOtherByProductId(int productId)
        {
            return await _ShopContext.ProductImages.Where(x => !x.IsDelete && !x.IsMain && x.ProductId == productId).ToListAsync();
        }

        public async Task<bool> Any(int id)
        {
            return await Get(id) != null;
        }
    }
}