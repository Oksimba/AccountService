using Entities;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        public IEnumerable<User> Get();

        public Task<User> Get(int id);

        public Task<User> GetByLogin(string login);

        public Task Create(User user);

        public Task Update(int UserId, User updatedUser);

        public Task<User> Delete(int UserId);

        public Task<bool> CheckIfUserExists(string login);

    }
}
