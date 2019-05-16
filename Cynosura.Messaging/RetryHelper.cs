using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cynosura.Messaging
{
    class RetryHelper
    {
        public static async Task TryAsync(Func<Task> action, int times, TimeSpan delay, ILogger logger)
        {
            var tryCount = 0;
            while (true)
            {
                try
                {
                    await action();
                    break;
                }
                catch (Exception e)
                {
                    tryCount++;
                    if (tryCount >= times)
                        throw;
                    logger.LogError(0, e, $"Try {tryCount} failed. Retrying in {delay}");
                    await Task.Delay(delay);
                }
            }
        }
    }
}
