using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class ServiceErrorWrapper
    {
        private readonly ILogger<ServiceErrorWrapper> _logger;

        public ServiceErrorWrapper(ILogger<ServiceErrorWrapper> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, UnitOfWork unitOfWork, string serviceName, string errorMessade)
        {
            using (var transaction = unitOfWork.BeginTransaction())
            {
                try
                {
                    var res = await action();
                    await transaction.CommitAsync();

                    return res;
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogCritical($"DataBase error in {serviceName}.");
                    _logger.LogCritical($"{errorMessade}: {dbEx.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Error in {serviceName}: {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task ExecuteAsync(Func<Task> action, UnitOfWork unitOfWork, string serviceName, string errorMessade)
        {
            using (var transaction = unitOfWork.BeginTransaction())
            {
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogCritical($"DataBase error in {serviceName}.");
                    _logger.LogCritical($"{errorMessade}: {dbEx.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Error in {serviceName}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
