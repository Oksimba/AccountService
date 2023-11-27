using AutoMapper;
using BusinessLogic.Interfaces;
using Common.Helpers;
using DataAccess;
using Entities;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using System.Numerics;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        UnitOfWork _unitOfWork;
        IMapper _mapper;
        ILogger<CardService> _logger;
        public UserService(UnitOfWork unitOfWork, IMapper mapper, ILogger<CardService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
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
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _unitOfWork.UserRepository.Create(user);

                    await transaction.CommitAsync();

                    _logger.LogInformation($"New user with id: {user.Id} was creared.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Exception while creating new user. {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task Update(int userId, User updatedAccount)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _unitOfWork.UserRepository.Update(userId, updatedAccount);

                    await transaction.CommitAsync();

                    _logger.LogInformation($"User with id: {updatedAccount.Id} was updated.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Exception while updating user with id: {userId}. {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<User> Delete(int userId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var deletedUser = await _unitOfWork.UserRepository.Delete(userId);

                    await transaction.CommitAsync();

                    _logger.LogInformation($"User with id: {userId} was deleted.");

                    return deletedUser;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Exception while deleting user with id: {userId}. {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> CheckIfUserExists(string login)
        {
            var user = await _unitOfWork.UserRepository.GetByLogin(login);
            return user != null;
        }
    }
}
