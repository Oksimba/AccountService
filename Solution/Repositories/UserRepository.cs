using DataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class UserRepository : IUserRepository

    {
        private AccountServiceDbContext context;

        public UserRepository(AccountServiceDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<User> Get()
        {
            return context.Users.Include(u => u.Cards);
        }

        public async Task<User> Get(int id)
        {
            return await context.Users
                .Include(u => u.Cards)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByLogin(string login)
        {
            return await context.Users
                .Include(u => u.Cards)
                .SingleOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User> Create(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task CreateMany(IEnumerable<User> users)
        {
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        public async Task Update(int id, User user)
        {
            User currentUser = await Get(id);
            currentUser.Login = user.Login;
            currentUser.HashPassword = user.HashPassword;
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.Email = user.Email;
            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.Cards = user.Cards;
            context.Users.Update(currentUser);
            await context.SaveChangesAsync();
        }

        public async Task<User> Delete(int id)
        {
            User user = await Get(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
            return user;
        }

        public async Task DeleteMany(IEnumerable<User> users)
        {
            if (users != null)
            {
                context.Users.RemoveRange(users);
                await context.SaveChangesAsync();
            }
        }
    }
}