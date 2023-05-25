using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class UserRepository : IDataRepository<User>
    {
        private readonly ShopContext _onlineShopContext;
        public UserRepository(ShopContext onlineShopContext)
        {
            _onlineShopContext = onlineShopContext;
        }

        public IEnumerable<User> GetAll()
        {
            return _onlineShopContext.Users.ToList();
        }

        public User Get(long id)
        {
            return _onlineShopContext.Users
                  .FirstOrDefault(e => e.Id == id);
        }

        public void Add(User entity)
        {
            entity.CreatedAt = DateTime.Now;
            //entity.CreateBy = entity.Id;
            _onlineShopContext.Users.Add(entity);
            _onlineShopContext.SaveChanges();
        }

        public void Update(User user, User entity)
        {
            user.FullName = entity.FullName;
            user.Email = entity.Email;
            user.Password = entity.Password;

            _onlineShopContext.SaveChanges();
        }

        public void Delete(User user)
        {
            //_onlineShopContext.Users.Remove(employee);
            user.IsDelete = true;
            _onlineShopContext.SaveChanges();
        }
    }
}
