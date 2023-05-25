using DomainModel;
using Framework;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserService : IUserService, IRegisterScoped
    {
        private readonly ShopContext _ShopContext;
        public UserService(ShopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task Add(User entity)
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

        public async Task Update(User Entity)
        {
            Entity.EditedAt = DateTime.Now;
            Entity.DeletedBy = null;
            Entity.DeletedAt = null;
            Entity.IsDelete = false;
            _ShopContext.Update(Entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Delete(User entity)
        {
            entity.DeletedAt = DateTime.Now;
            entity.IsDelete = true;
            await _ShopContext.SaveChangesAsync();
        }

        public async Task<User?> Get(int id)
        {
            var _Category = new User();
            if (id.IsNotZero())
            {
                _Category = await _ShopContext.Users.Include(u => u.UserType).FirstOrDefaultAsync(x=> x.Id == id);
                if (_Category != null && !_Category.IsDelete)
                    return _Category;
            }
            return _Category;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _ShopContext.Users.Where(x => !x.IsDelete).Include(u => u.UserType).ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email, string password)
        {
            password = password.Decrypt();
            var user = await _ShopContext.Users.FirstOrDefaultAsync(c => c.Email == email && c.Password == password && !c.IsDelete);
            return user;
        }

        public async Task<User?> CheckToken(string token, string? permission = null)
        {
            var user = await _ShopContext.UserLogins.FirstOrDefaultAsync(x => x.Token == token && x.ExpireDate > DateTime.Now);
            if (user != null)
                return await _ShopContext.Users.FirstOrDefaultAsync(c => c.Id == user.UserId);

            return new User();
        }

        public async Task<IEnumerable<User>> GetByUser(int userId)
        {
            return await _ShopContext.Users.Where(c => c.Id == userId && !c.IsDelete).ToListAsync();
        }

        public async Task<bool> Any(int id)
        {
            return await Get(id) != null;
        }
    }
}