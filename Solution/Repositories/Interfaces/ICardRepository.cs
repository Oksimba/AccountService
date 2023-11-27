using Entities;

namespace Repositories.Interfaces
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAll(int userId);
        Task<Card> Get(int id);
        Task Create(Card cards);
        Task CreateMany(IEnumerable<Card> cards);
        Task Update(int id, Card card);
        Task<Card> Delete(int id);
        Task DeleteMany(IEnumerable<Card> cards);

        bool IsUserCardOwner(int userId, int orderId);
    }
}
