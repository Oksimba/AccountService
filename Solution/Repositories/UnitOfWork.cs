using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Interfaces;

namespace DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private readonly AccountServiceDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ICardRepository _cardRepository;


        public UnitOfWork(AccountServiceDbContext context,
            IUserRepository userRepository,
            ICardRepository cardRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _cardRepository = cardRepository;
        }


        
        public IUserRepository UserRepository => _userRepository;
        public ICardRepository CardRepository => _cardRepository;



        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool EnsureDataBaseDeleted()
        {
            return _context.Database.EnsureDeleted();
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
