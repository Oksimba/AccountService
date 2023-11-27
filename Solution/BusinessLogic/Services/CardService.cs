using AutoMapper;
using BusinessLogic.Interfaces;
using DataAccess;
using Entities;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BusinessLogic.Services
{
    public class CardService : ICardService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        ILogger<CardService> _logger;
        public CardService(UnitOfWork unitOfWork, IMapper mapper, ILogger<CardService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public IEnumerable<Card> GetAll(int userId)
        {
            var users = _unitOfWork.CardRepository.GetAll(userId);
            return users;
        }

        public Task<Card> Get(int id)
        {
            return _unitOfWork.CardRepository.Get(id);
        }

        public async Task Create(Card card)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _unitOfWork.CardRepository.Create(card);

                    await transaction.CommitAsync();

                    _logger.LogInformation($"New card with id: {card.Id} was creared.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Exception while creating new card. {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task Update(int id, Card updatedCard)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _unitOfWork.CardRepository.Update(id, updatedCard);

                    await transaction.CommitAsync();

                    _logger.LogInformation($"Card with id: {updatedCard.Id} was updated.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Exception while updating the card with id: {id}. {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Card> Delete(int id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var deletedCard = await _unitOfWork.CardRepository.Delete(id);

                    await transaction.CommitAsync();

                    _logger.LogInformation($"Card with id: {id} was deleted.");

                    return deletedCard;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Exception while deleting the card with id: {id}. {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public bool IsUserCardOwner(int userId, int cardId)
        {
            return _unitOfWork.CardRepository.IsUserCardOwner(userId, cardId);
        }
    }
}
