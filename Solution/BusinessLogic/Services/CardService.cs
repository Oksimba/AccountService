using AutoMapper;
using BusinessLogic.Interfaces;
using DataAccess;
using Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class CardService : ICardService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        ILogger<CardService> _logger;
        ServiceErrorWrapper _errorWrapper;

        string _entityName = nameof(Card);
        string _servicName = nameof(CardService);
        public CardService(UnitOfWork unitOfWork, IMapper mapper, ILogger<CardService> logger, ServiceErrorWrapper errorWrapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _errorWrapper = errorWrapper;
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
            await _errorWrapper.ExecuteAsync(async () =>
            {
                await _unitOfWork.CardRepository.Create(card);

                _logger.LogInformation($"New card with id: {card.Id} was creared.");

                _logger.LogInformation(LogMessages.OnEntityCreatingLog(card.Id, _entityName));
            },
            _unitOfWork,
            _servicName,
            LogMessages.OnEntityCreatingErrorLog(_entityName));

        }

        public async Task Update(int id, Card updatedCard)
        {
            await _errorWrapper.ExecuteAsync(async () =>
            {
                await _unitOfWork.CardRepository.Update(id, updatedCard);

                _logger.LogInformation(LogMessages.OnEntityUpdatingLog(id, _entityName));
            },
            _unitOfWork,
            _servicName,
            LogMessages.OnEntityUpdatingErrorLog(id, _entityName));
        }

        public async Task<Card> Delete(int id)
        {

            return await _errorWrapper.ExecuteAsync(async () =>
            {
                var deletedCard = await _unitOfWork.CardRepository.Delete(id);

                _logger.LogInformation(LogMessages.OnEntityDeletingLog(id, _entityName));

                return deletedCard;
            },
            _unitOfWork,
            _servicName,
            LogMessages.OnEntityDeletingErrorLog(id, _entityName));
        }

        public bool IsUserCardOwner(int userId, int cardId)
        {
            return _unitOfWork.CardRepository.IsUserCardOwner(userId, cardId);
        }
    }
}
