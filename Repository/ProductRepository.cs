using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class ProductRepository : IDataRepository<Product>
    {
        private readonly OnlineShopContext _onlineShopContext;
        public ProductRepository(OnlineShopContext onlineShopContext)
        {
            _onlineShopContext = onlineShopContext;
        }

        public IEnumerable<Product> GetAll()
        {
            return _onlineShopContext.Products.ToList();
        }

        public IEnumerable<Product> GetByCategory(int categoryId, int count)
        {
            return _onlineShopContext.Products.Where(c=>c.CategoryId==categoryId && c.IsDelete==false)
                .OrderByDescending(c=>c.Id).Take(count).ToList();
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            return _onlineShopContext.Products.Where(c => c.CategoryId == categoryId && c.IsDelete == false)
                .OrderByDescending(c => c.Id).ToList();
        }

        public Product Get(long id)
        {
            return _onlineShopContext.Products
                  .FirstOrDefault(e => e.Id == id);
        }

        public void Add(Product entity)
        {
            entity.CreateDate = DateTime.Now;
            _onlineShopContext.Products.Add(entity);
            _onlineShopContext.SaveChanges();
        }

        public void Update(Product dbEntity, Product entity)
        {
            dbEntity.CategoryId = entity.CategoryId;
            dbEntity.Title = entity.Title;
            dbEntity.Price = entity.Price;
            dbEntity.Quantity = entity.Quantity;
            dbEntity.Description = entity.Description;

            _onlineShopContext.SaveChanges();
        }

        public void Delete(Product entity)
        {
            entity.IsDelete = true;
            _onlineShopContext.SaveChanges();
        }
    }
}
