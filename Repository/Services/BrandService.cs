using DomainModel;
using Framework;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class BrandService : IDataService<Brand>, IRegisterScoped
    {
        private readonly ShopContext _ShopContext;
        public BrandService(ShopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task Add(Brand entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.EditedBy = null;
            entity.CreatedAt = entity.CreatedAt;
            entity.DeletedBy = null;
            entity.DeletedAt = null;
            entity.IsDelete = false;
            await _ShopContext.Brands.AddAsync(entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Update(Brand Entity)
        {
            Entity.EditedAt = DateTime.Now;
            Entity.DeletedBy = null;
            Entity.DeletedAt = null;
            Entity.IsDelete = false;
            _ShopContext.Update(Entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Delete(Brand entity)
        {
            entity.DeletedAt = DateTime.Now;
            entity.IsDelete = true;
            await _ShopContext.SaveChangesAsync();
        }

        public async Task<Brand?> Get(int id)
        {
            var _Brand = new Brand();
            if (id.IsNotZero())
            {
                _Brand = await _ShopContext.Brands.FindAsync(id);
                if (_Brand != null && !_Brand.IsDelete)
                    return _Brand;
            }
            return _Brand;
        }

        public async Task<IEnumerable<Brand>> GetAll()
        {
            return await _ShopContext.Brands.Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<IEnumerable<Brand>> GetByUser(int userId)
        {
            return await _ShopContext.Brands.Where(x => !x.IsDelete && x.CreatedBy == userId).ToListAsync();
        }

        public async Task<bool> Any(int id)
        {
            return await Get(id) != null;
        }
    }
}