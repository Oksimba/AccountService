using AutoMapper;
using BusinessLogic.Interfaces;
using DataAccess;
using Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        ILogger<CardService> _logger;
        ServiceErrorWrapper _errorWrapper;
        string _servicName = nameof(UserService);

        public UserService(UnitOfWork unitOfWork, 
            IMapper mapper, ILogger<CardService> logger, 
            ServiceErrorWrapper serviceErrorWrapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _errorWrapper = serviceErrorWrapper;
        }

        public IEnumerable<User> Get()
        {
            var users = _unitOfWork.UserRepository.Get();
            
            return users;
        }

        public async Task<User> Get(int id)
        {
            var user = await _unitOfWork.UserRepository.Get(id);
            return user;
        }

        public async Task<User> GetByLogin(string login)
        {
            var user = await _unitOfWork.UserRepository.GetByLogin(login);
            return user;
        }

        public async Task Create(User user)
        {
            await _errorWrapper.ExecuteAsync(async () =>
            {
                await _unitOfWork.UserRepository.Create(user);

                _logger.LogInformation(LogMessages.OnEntityCreatingLog);
            },
            _unitOfWork,
            _servicName,
            LogMessages.OnEntityCreatingErrorLog);
        }

        public async Task Update(int userId, User updatedAccount)
        {
            await _errorWrapper.ExecuteAsync(async () =>
            {
                await _unitOfWork.UserRepository.Update(userId, updatedAccount);

                _logger.LogInformation(LogMessages.OnEntityUpdatingLog);
            },
            _unitOfWork,
            _servicName,
            LogMessages.OnEntityUpdatingErrorLog);
        }

        public async Task<User> Delete(int userId)
        {
            return await _errorWrapper.ExecuteAsync(async () =>
            {
                var deletedUser = await _unitOfWork.UserRepository.Delete(userId);

                _logger.LogInformation(LogMessages.OnEntityDeletingLog);

                return deletedUser;
            },
            _unitOfWork,
            _servicName,
            LogMessages.OnEntityDeletingErrorLog);
        }

        public async Task<bool> CheckIfUserExists(string login)
        {
            var user = await _unitOfWork.UserRepository.GetByLogin(login);
            return user != null;
        }
    }
}
