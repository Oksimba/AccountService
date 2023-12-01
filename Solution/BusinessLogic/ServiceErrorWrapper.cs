using Common.Exceptions;
using DataAccess;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class ServiceErrorWrapper
    {
        private readonly ILogger<ServiceErrorWrapper> _logger;

        public ServiceErrorWrapper(ILogger<ServiceErrorWrapper> logger)
        {
            _logger = logger;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, UnitOfWork unitOfWork, string serviceName, string errorMessage)
        {
            using (var transaction = unitOfWork.BeginTransaction())
            {
                try
                {
                    var res = await action();
                    await transaction.CommitAsync();

                    return res;
                }
                catch (RepositoryException repoEx)
                {
                    _logger.LogCritical($"Repository error in {serviceName}.");
                    _logger.LogCritical($"{errorMessage}: {repoEx.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Unexpected error in {serviceName}: {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task ExecuteAsync(Func<Task> action, UnitOfWork unitOfWork, string serviceName, string errorMessage)
        {
            using (var transaction = unitOfWork.BeginTransaction())
            {
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch (RepositoryException repoEx)
                {
                    _logger.LogCritical($"Repository error in {serviceName}.");
                    _logger.LogCritical($"{errorMessage}: {repoEx.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Unexpected error in {serviceName}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
