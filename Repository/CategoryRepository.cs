using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Repository
{
    public class CategoryRepository : IDataRepository<Category>
    {
        private readonly ShopContext _onlineShopContext;
        public CategoryRepository(ShopContext onlineShopContext)
        {
            _onlineShopContext = onlineShopContext;
        }

        public void Add(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public Category Get(long id)
        {
            return _onlineShopContext.Categories.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Category dbEntity, Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
