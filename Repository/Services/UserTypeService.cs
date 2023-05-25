using DomainModel;
using Framework;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserTypeService : IDataService<UserType>, IRegisterScoped
    {
        private readonly ShopContext _ShopContext;
        public UserTypeService(ShopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task Add(UserType entity)
        {
            await _ShopContext.UserTypes.AddAsync(entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Update(UserType Entity)
        {
            _ShopContext.Update(Entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Delete(UserType entity)
        {
            _ShopContext.Remove(entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task<UserType?> Get(int id)
        {
            var _UserType = new UserType();
            if (id.IsNotZero())
            {
                _UserType = await _ShopContext.UserTypes.FindAsync(id);
                if (_UserType != null)
                    return _UserType;
            }
            return _UserType;
        }

        public async Task<IEnumerable<UserType>> GetAll()
        {
            return await _ShopContext.UserTypes.ToListAsync();
        }

        public async Task<IEnumerable<UserType>> GetByUser(int userId)
        {
            return await _ShopContext.UserTypes.Include(x => x.Users).Where(x => !x.Users.Any(y => y.Id == userId)).ToListAsync();
        }

        public async Task<bool> Any(int id)
        {
            return (await Get(id) != null);
        }
    }
}