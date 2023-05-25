using DomainModel;
using Framework;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrderService : IDataService<Order>, IRegisterScoped
    {
        private readonly ShopContext _ShopContext;
        public OrderService(ShopContext ShopContext)
        {
            _ShopContext = ShopContext;
        }

        public async Task Add(Order entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.EditedBy = null;
            entity.CreatedAt = entity.CreatedAt;
            entity.DeletedBy = null;
            entity.DeletedAt = null;
            entity.IsDelete = false;
            await _ShopContext.Orders.AddAsync(entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Update(Order Entity)
        {
            Entity.EditedAt = DateTime.Now;
            Entity.DeletedBy = null;
            Entity.DeletedAt = null;
            Entity.IsDelete = false;
            _ShopContext.Update(Entity);
            await _ShopContext.SaveChangesAsync();
        }

        public async Task Delete(Order entity)
        {
            entity.DeletedAt = DateTime.Now;
            entity.IsDelete = true;
            await _ShopContext.SaveChangesAsync();
        }

        public async Task<Order?> Get(int id)
        {
            var _Order = new Order();
            if (id.IsNotZero())
            {
                _Order = await _ShopContext.Orders.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
                if (_Order != null && !_Order.IsDelete)
                    return _Order;
            }
            return _Order;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _ShopContext.Orders.Include(x => x.User).Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByUser(int userId)
        {
            return await _ShopContext.Orders.Include(x => x.User).Where(x => !x.IsDelete && x.CreatedBy == userId).ToListAsync();
        }

        public async Task<bool> Any(int id)
        {
            return await Get(id) != null;
        }
    }
}