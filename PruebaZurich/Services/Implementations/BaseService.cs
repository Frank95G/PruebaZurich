using PruebaZurich.Data.Repositories.Interfaces;

namespace PruebaZurich.Services.Implementations
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;

        protected BaseService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<T> ExecuteWithTransactionAsync<T>(Func<Task<T>> action, string operationName)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await action();
                await _unitOfWork.CommitTransactionAsync(transaction);
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(transaction);
                _logger.LogError(ex, "Error durante la operación transaccional");
                throw;
            }
        }
    }
}
