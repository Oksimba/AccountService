using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Validation;

namespace Repositories
{
    public class RepositotyErrorWrapper
    {
        private readonly ILogger<RepositotyErrorWrapper> _logger;

        public RepositotyErrorWrapper(ILogger<RepositotyErrorWrapper> logger)
        {
            _logger = logger;
        }
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, string repoName, string errorMessage)
        {

            try
            {
                var res = await action();

                return res;
            }
            catch (DbUpdateException dbEx)
            {
                var error = $"DataBase error in {repoName}.";
                _logger.LogCritical(error);
                _logger.LogCritical($"{errorMessage}: {dbEx.Message}");
                throw new RepositoryException(error);
            }
            catch(DbEntityValidationException ex)
            {
                var error = $"Validation error in {repoName}.";
                _logger.LogCritical(error);
                _logger.LogCritical($"{errorMessage}: {ex.Message}");
                throw new RepositoryException(error);
            }
            catch(InvalidOperationException ex)
            {
                var error = $"Invalid operation error in {repoName}.";
                _logger.LogCritical(error);
                _logger.LogCritical($"{errorMessage}: {ex.Message}");
                throw new RepositoryException(error);
            }
            catch (Exception ex)
            {
                var error = $"Unexpected error in {repoName}: {ex.Message}";
                _logger.LogCritical(error);
                throw new RepositoryException(error);
            }
        }

        public async Task ExecuteAsync(Func<Task> action, string repoName, string errorMessage)
        {

            try
            {
                await action();
            }
            catch (DbUpdateException dbEx)
            {
                var error = $"DataBase error in {repoName}.";
                _logger.LogCritical(error);
                _logger.LogCritical($"{errorMessage}: {dbEx.Message}");
                throw new RepositoryException(error);
            }
            catch (DbEntityValidationException ex)
            {
                var error = $"Validation error in {repoName}.";
                _logger.LogCritical(error);
                _logger.LogCritical($"{errorMessage}: {ex.Message}");
                throw new RepositoryException(error);
            }
            catch (InvalidOperationException ex)
            {
                var error = $"Invalid operation error in {repoName}.";
                _logger.LogCritical(error);
                _logger.LogCritical($"{errorMessage}: {ex.Message}");
                throw new RepositoryException(error);
            }
            catch (Exception ex)
            {
                var error = $"Unexpected error in {repoName}: {ex.Message}";
                _logger.LogCritical(error);
                throw new RepositoryException(error);
            }
        }
    }
}
