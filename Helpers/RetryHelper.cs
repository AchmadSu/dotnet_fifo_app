using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Helpers
{
    public static class RetryHelper
    {
        public static readonly Func<Exception, bool> DefaultTransientExceptionFilter = e =>
        {
            if (e is DbUpdateConcurrencyException)
                return true;

            if (e is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505")
                return true;

            return false;
        };

        public static async Task<OperationResult<T>> RetryOperationAsync<T>(
            Func<Task<OperationResult<T>>> operation,
            int maxRetry = 3,
            int retryDelay = 50,
            ILogger? logger = null,
            Func<Exception, bool>? transientExceptionFilter = null
        )
        {
            int attempt = 0;
            while (true)
            {
                attempt++;
                try
                {
                    return await operation();
                }
                catch (Exception e) when (attempt < maxRetry && (transientExceptionFilter?.Invoke(e) ?? true))
                {
                    logger?.LogWarning(e, $"Retry attempt {attempt} due to transient exception.");
                    await Task.Delay(retryDelay * attempt);
                }
                catch (Exception e)
                {
                    logger?.LogError(e, $"Operation failed on attempt {attempt}");
                    return OperationResult<T>.InternalServerError("Operation failed due to unexpected error");
                }
            }
        }

        public static async Task<OperationResult<T>> RetryOperationWithTransactionAsync<T>(
            ApplicationDBContext context,
            Func<Task<OperationResult<T>>> operation,
            int maxRetry = 3,
            int retryDelayMs = 50,
            ILogger? logger = null,
            Func<Exception, bool>? transientExceptionFilter = null
        )
        {
            int attempt = 0;

            while (true)
            {
                attempt++;
                await using var trx = await context.Database.BeginTransactionAsync();
                try
                {
                    var result = await operation();

                    if (result.IsSuccess)
                    {
                        await trx.CommitAsync();
                    }
                    else
                    {
                        await trx.RollbackAsync();
                    }

                    return result;
                }
                catch (Exception e) when (attempt < maxRetry && (transientExceptionFilter?.Invoke(e) ?? true))
                {
                    await trx.RollbackAsync();
                    logger?.LogWarning(e, $"Retry attempt {attempt} due to transient exception.");
                    await Task.Delay(retryDelayMs * attempt);
                }
                catch (Exception e)
                {
                    await trx.RollbackAsync();
                    logger?.LogError(e, $"Operation failed on attempt {attempt}");
                    return OperationResult<T>.InternalServerError("Operation failed due to unexpected error");
                }
            }
        }
    }
}