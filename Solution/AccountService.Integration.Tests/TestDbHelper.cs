using Common.Helpers;
using DataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AccountService.Integration.Tests
{
    internal class TestDbHelper
    {
        private static AccountServiceDbContext Context { get; set; }

        public TestDbHelper(AccountServiceDbContext context)
        {
            Context = context;
        }
        public void CreateTestDb()
        {
            Context.Database.Migrate();
            SeedData();
        }

        private static void SeedData()
        {
            RemoveData();
            SeedUsers();
            SeedCards();
            Context.SaveChanges();
        }


        private static void SeedUsers()
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

            Context.Users.AddRange(users);
        }

        private static void SeedCards()
        {
            var cards = new List<Card>();
            for (int i = 0; i < 9; i++)
            {
                cards.Add(
                    new Card
                    {
                        UserId = Context.Users.First(u => u.Login == $"user{i}").Id,
                        CardNumber = new string((char)i, 8) + new string((char)(i + 1), 8)
                    });

                cards.Add(
                    new Card
                    {
                        UserId = Context.Users.First(u => u.Login == $"user{i}").Id,
                        CardNumber = new string((char)(i + 1), 8) + new string((char)i, 8)
                    });
            }

            Context.Cards.AddRange(cards);
        }

        private static void RemoveData()
        {
            Context.RemoveRange(Context.Cards);
            Context.RemoveRange(Context.Users);
            Context.SaveChanges();
        }

    }
}
