using Entities;
namespace BusinessLogic.Interfaces
{
    public interface ICardService
    {
        public IEnumerable<Card> GetAll(int userId);

        public Task<Card> Get(int id);

        public Task Create(Card card);

        public Task Update(int id, Card updatedCard);

        public Task<Card> Delete(int id);

        public bool IsUserCardOwner(int userId, int orderId);

    }
}
