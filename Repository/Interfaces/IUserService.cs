using DomainModel;

namespace Repository
{
    public interface IUserService : IDataService<User>
    {
        Task<User?> GetUserByEmail(string email, string password);
        Task<User?> CheckToken(string token, string? permission = null);
    }
}