using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class ProductImagesRepository : IDataRepository<ProductImages>
    {
        private readonly OnlineShopContext _onlineShopContext;
        public ProductImagesRepository(OnlineShopContext onlineShopContext)
        {
            _onlineShopContext = onlineShopContext;
        }

        public IEnumerable<ProductImages> GetAll()
        {
            return _onlineShopContext.GetProductImages.ToList();
        }

        public ProductImages Get(long id)
        {
            return _onlineShopContext.GetProductImages
                  .FirstOrDefault(e => e.Id == id);
        }
        public ProductImages GetByProductId(int ProductId)
        {
            return _onlineShopContext.GetProductImages
                  .FirstOrDefault(e => e.ProductId == ProductId && e.IsMain);
        }

        public void Add(ProductImages entity)
        {
            entity.CreateDate = DateTime.Now;
            _onlineShopContext.GetProductImages.Add(entity);
            _onlineShopContext.SaveChanges();
        }

        public void Update(ProductImages dbEntity, ProductImages entity)
        {
            dbEntity.Title = entity.Title;

            _onlineShopContext.SaveChanges();
        }

        public void Delete(ProductImages entity)
        {
            entity.IsDelete = true;
            _onlineShopContext.SaveChanges();
        }

    }
}
