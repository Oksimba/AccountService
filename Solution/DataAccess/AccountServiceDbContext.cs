using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AccountServiceDbContext : DbContext, IDisposable
    {
        public AccountServiceDbContext(DbContextOptions<AccountServiceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Dispose();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            CreateAccountCardsRelations(modelBuilder);
        }

        private void CreateAccountCardsRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasIndex(s => s.CardNumber).IsUnique();

            modelBuilder.Entity<Card>()
                .HasOne(s => s.User)
                .WithMany(s => s.Cards)
                .HasForeignKey(s => s.UserId);
        }
    }
}
