namespace Repository
{
    public interface IDataService<TEntity>
    {
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        Task<TEntity?> Get(int id);
        Task<bool> Any(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetByUser(int userId);
    }
}