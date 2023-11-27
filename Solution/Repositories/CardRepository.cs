using DataAccess;
using Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class CardRepository : ICardRepository
    {
        private AccountServiceDbContext context;

        public CardRepository(AccountServiceDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Card> GetAll(int userId)
        {
            return context.Cards.Where(c => c.UserId == userId);
        }

        public async Task<Card> Get(int id)
        {
            return await context.Cards.FindAsync(id);
        }

        public async Task Create(Card card)
        {
            await context.Cards.AddAsync(card);
            await context.SaveChangesAsync();
        }

        public async Task CreateMany(IEnumerable<Card> cards)
        {
            await context.Cards.AddRangeAsync(cards);
            await context.SaveChangesAsync();
        }

        public async Task Update(int id, Card card)
        {
            Card currentCard = await Get(id);
            currentCard.CardBalance = card.CardBalance;
            context.Cards.Update(currentCard);
            await context.SaveChangesAsync();
        }

        public async Task<Card> Delete(int id)
        {
            Card card = await Get(id);
            if (card != null)
            {
                context.Cards.Remove(card);
                await context.SaveChangesAsync();
            }
            return card;
        }

        public async Task DeleteMany(IEnumerable<Card> cards)
        {
            if (cards != null)
            {
                context.Cards.RemoveRange(cards);
                await context.SaveChangesAsync();
            }
        }

        public bool IsUserCardOwner(int userId, int cardId)
        {
            var card = context.Cards.FirstOrDefault(c => c.Id == cardId);

            return card != null && card.UserId == userId;
        }
    }
}
