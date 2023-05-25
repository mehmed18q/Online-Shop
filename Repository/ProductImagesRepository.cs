using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class ProductImagesRepository : IDataRepository<ProductImage>
    {
        private readonly ShopContext _onlineShopContext;
        public ProductImagesRepository(ShopContext onlineShopContext)
        {
            _onlineShopContext = onlineShopContext;
        }

        public IEnumerable<ProductImage> GetAll()
        {
            return _onlineShopContext.ProductImages.ToList();
        }

        public ProductImage Get(long id)
        {
            return _onlineShopContext.ProductImages
                  .FirstOrDefault(e => e.Id == id);
        }
        public ProductImage GetByProductId(int ProductId)
        {
            return _onlineShopContext.ProductImages
                  .FirstOrDefault(e => e.ProductId == ProductId && e.IsMain);
        }

        public void Add(ProductImage entity)
        {
            entity.CreatedAt = DateTime.Now;
            _onlineShopContext.ProductImages.Add(entity);
            _onlineShopContext.SaveChanges();
        }

        public void Update(ProductImage dbEntity, ProductImage entity)
        {
            dbEntity.Title = entity.Title;

            _onlineShopContext.SaveChanges();
        }

        public void Delete(ProductImage entity)
        {
            entity.IsDelete = true;
            _onlineShopContext.SaveChanges();
        }

    }
}
