using Entities;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> Get();
        Task<User> Get(int id);
        Task<User> GetByLogin(string login);
        Task<User> Create(User user);
        Task CreateMany(IEnumerable<User> users);
        Task Update(int id, User user);
        Task<User> Delete(int id);
        Task DeleteMany(IEnumerable<User> users);
    }
}
