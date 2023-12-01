using DataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;

namespace Repositories
{
    public class UserRepository : IUserRepository

    {
        private AccountServiceDbContext _context;
        private ILogger<UserRepository> _logger;
        private RepositotyErrorWrapper _errorWrapper;
        private string _repoName = nameof(UserRepository); 
        private string _entityName = nameof(User);

        public UserRepository(AccountServiceDbContext context, ILogger<UserRepository> logger, RepositotyErrorWrapper errorWrapper)
        {
            this._context = context;
            this._logger = logger;
            this._errorWrapper = errorWrapper;
        }
        public IEnumerable<User> Get()
        {
            return _context.Users.Include(u => u.Cards);
        }

        public async Task<User> Get(int id)
        {
            return await _errorWrapper.ExecuteAsync(async () =>
            {
                return await _context.Users
                .Include(u => u.Cards)
                .SingleOrDefaultAsync(u => u.Id == id);
            },
            _repoName,
            LogMessages.OnGetEntityByIdErrorLog(id, _entityName)
            );
        }
    

        public async Task<User> GetByLogin(string login)
        {
            return await _errorWrapper.ExecuteAsync(async () =>
            {
                return await _context.Users
                .Include(u => u.Cards)
                .SingleOrDefaultAsync(u => u.Login == login);
            },
            _repoName,
            LogMessages.OnGetUserByLoginErrorLog(login));
        }

        public async Task<User> Create(User user)
        {
            return await _errorWrapper.ExecuteAsync(async () =>
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation(LogMessages.OnEntityCreatingLog(user.Id, _entityName));
                return user;
            },
            _repoName,
            LogMessages.OnEntityCreatingErrorLog(_entityName)
            );
        }

        public async Task CreateMany(IEnumerable<User> users)
        {
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, User user)
        {
            await _errorWrapper.ExecuteAsync(async () =>
            {
                User currentUser = await Get(id);
                currentUser.Login = user.Login;
                currentUser.HashPassword = user.HashPassword;
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Email = user.Email;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Cards = user.Cards;
                _context.Users.Update(currentUser);
                _logger.LogInformation(LogMessages.OnEntityUpdatingLog(user.Id, _entityName));
                await _context.SaveChangesAsync();
            },
            _repoName,
            LogMessages.OnEntityUpdatingErrorLog(id, _entityName));
        }

        public async Task<User> Delete(int id)
        {
            return await _errorWrapper.ExecuteAsync(async () =>
            {
                User user = await Get(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
                _logger.LogInformation(LogMessages.OnEntityDeletingLog(user.Id, _entityName));
                return user;
            },
            _repoName,
            LogMessages.OnEntityDeletingErrorLog(id, _entityName)
            );
        }

        public async Task DeleteMany(IEnumerable<User> users)
        {
            if (users != null)
            {
                _context.Users.RemoveRange(users);
                await _context.SaveChangesAsync();
            }
        }
    }
}