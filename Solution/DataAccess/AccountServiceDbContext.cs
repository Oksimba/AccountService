using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AccountServiceDbContext : DbContext
    {
        public AccountServiceDbContext(DbContextOptions<AccountServiceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateAccountCardsRelations(modelBuilder);
        }

        private void CreateAccountCardsRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasOne(s => s.User)
                .WithMany(s => s.Cards)
                .HasForeignKey(s => s.UserId);
        }
    }
}
