namespace Wrecept.Core.Services;

using Wrecept.Core.Exceptions;

internal static class ServiceUtil
{
    public static async Task<TResult> WrapAsync<TResult>(Func<Task<TResult>> action, string message)
    {
        try
        {
            return await action().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new ServiceException(message, ex);
        }
    }

    public static async Task WrapAsync(Func<Task> action, string message)
    {
        try
        {
            await action().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new ServiceException(message, ex);
        }
    }
}
