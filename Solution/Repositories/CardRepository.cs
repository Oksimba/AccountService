using DataAccess;
using Entities;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;

namespace Repositories
{
    public class CardRepository : ICardRepository
    {
        private AccountServiceDbContext context;

        private ILogger<CardRepository> _logger;
        private RepositotyErrorWrapper _errorWrapper;
        private string _repoName = nameof(CardRepository);
        private string _entityName = nameof(Card);


        public CardRepository(AccountServiceDbContext context, RepositotyErrorWrapper errorWrapper, ILogger<CardRepository> logger)
        {
            this.context = context;
            _errorWrapper = errorWrapper;
            _logger = logger;
        }
        public IEnumerable<Card> GetAll(int userId)
        {
            return context.Cards.Where(c => c.UserId == userId);
        }

        public async Task<Card> Get(int id)
        {
            return await _errorWrapper.ExecuteAsync(async () =>
            {
                return await context.Cards.FindAsync(id);
            },
            _repoName,
            LogMessages.OnGetEntityByIdErrorLog(id, _entityName)
            );
        }

        public async Task Create(Card card)
        {
            await _errorWrapper.ExecuteAsync(async () =>
            {
                await context.Cards.AddAsync(card);
                await context.SaveChangesAsync();
                _logger.LogInformation(LogMessages.OnEntityCreatingLog(card.Id, _entityName));
            },
            _repoName,
            LogMessages.OnEntityCreatingErrorLog(_entityName));
        }

        public async Task CreateMany(IEnumerable<Card> cards)
        {
            await context.Cards.AddRangeAsync(cards);
            await context.SaveChangesAsync();
        }

        public async Task Update(int id, Card card)
        {
            await _errorWrapper.ExecuteAsync(async () =>
            {
                Card currentCard = await Get(id);
                currentCard.CardBalance = card.CardBalance;
                context.Cards.Update(currentCard);
                await context.SaveChangesAsync();
                _logger.LogInformation(LogMessages.OnEntityUpdatingLog(card.Id, _entityName));
            },
            _repoName,
            LogMessages.OnEntityUpdatingErrorLog(id, _entityName));
        }

        public async Task<Card> Delete(int id)
        {
            return await _errorWrapper.ExecuteAsync(async () =>
            {
                Card card = await Get(id);
                if (card != null)
                {
                    context.Cards.Remove(card);
                    await context.SaveChangesAsync();
                    _logger.LogInformation(LogMessages.OnEntityDeletingLog(card.Id, _entityName));
                }
                return card;
            },
            _repoName,
            LogMessages.OnEntityDeletingErrorLog(id, _entityName)
            );
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
