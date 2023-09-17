using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cynosura.Core.Messaging
{
    public interface IMessagingService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);

        Task PublishAsync<T>(T message) where T : class;
        Task PublishAsync<T>(T message, TimeSpan timeToLive) where T : class;
        Task SendAsync<T>(string queue, T message) where T : class;
        Task<TResponse> RequestAsync<TRequest, TResponse>(string queue, TRequest message) 
            where TRequest : class 
            where TResponse : class;
    }
}
