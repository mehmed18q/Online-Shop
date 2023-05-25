using DomainModel;
using Framework;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CategoryService : IDataService<Category>, IRegisterScoped
    {
        private readonly ShopContext _ShopContext;
        public CategoryService(ShopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task Add(Category entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.EditedBy = null;
            entity.CreatedAt = entity.CreatedAt;
            entity.DeletedBy = null;
            entity.DeletedAt = null;
            entity.IsDelete = false;
            await _ShopContext.Categories.AddAsync(entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Update(Category Entity)
        {
            Entity.EditedAt = DateTime.Now;
            Entity.DeletedBy = null;
            Entity.DeletedAt = null;
            Entity.IsDelete = false;
            _ShopContext.Update(Entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Delete(Category entity)
        {
            entity.DeletedAt = DateTime.Now;
            entity.IsDelete = true;
            await _ShopContext.SaveChangesAsync();
        }

        public async Task<Category?> Get(int id)
        {
            var _Category = new Category();
            if (id.IsNotZero())
            {
                _Category = await _ShopContext.Categories.FindAsync(id);
                if (_Category != null && !_Category.IsDelete)
                    return _Category;
            }
            return _Category;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _ShopContext.Categories.Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetByUser(int userId)
        {
            return await _ShopContext.Categories.Where(x => !x.IsDelete && x.CreatedBy == userId).ToListAsync();
        }

        public async Task<bool> Any(int id)
        {
            return (await Get(id) != null);
        }
    }
}