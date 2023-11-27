using BusinessLogic.Interfaces;
using Common.Helpers;
using DataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class DbInitializeService: IDbInitializeService
    {
        public AccountServiceDbContext Ctx { get; private set; }
        public DbInitializeService(AccountServiceDbContext ctx)
        {
            this.Ctx = ctx;
        }
        public void Initialize()
        {
            Ctx.Database.EnsureDeleted();
            Ctx.Database.Migrate();
            SeedData();
            Ctx.SaveChanges();
        }

        private void SeedData()
        {
            RemoveData();
            SeedUsers();
            SeedCards();
        }


        private void SeedUsers()
        {
            var users = new List<User>();
            for (int i = 0; i < 9; i++)
            {
                users.Add(
                    new User
                    {
                        Login = $"user{i}",
                        HashPassword = AuthHelper.GetHashString($"password{i}"),
                        FirstName = $"User{i}Name",
                        LastName = $"User{i}Surname",
                        Email = $"user{i}@gmail.com",
                        PhoneNumber = $"+380-" + new string((char)i, 9)
                    });
            }

            Ctx.Users.AddRange(users);
            Ctx.SaveChanges();
        }

        private void SeedCards()
        {
            var cards = new List<Card>();
            for (int i = 0; i < 9; i++)
            {
                cards.Add(
                    new Card
                    {
                        UserId = Ctx.Users.First(u => u.Login == $"user{i}").Id,
                        CardNumber = new string((char)i, 8) + new string((char)(i+1), 8)
                    });

                cards.Add(
                    new Card
                    {
                        UserId = Ctx.Users.First(u => u.Login == $"user{i}").Id,
                        CardNumber = new string((char)(i + 1), 8) + new string((char)i, 8)
                    });
            }

            Ctx.Cards.AddRange(cards);
            Ctx.SaveChanges();
        }
        
        private void RemoveData()
        {
            Ctx.RemoveRange(Ctx.Cards);
            Ctx.RemoveRange(Ctx.Users);
            Ctx.SaveChanges();
        }
    }
}
