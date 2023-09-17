using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Core.Messaging
{
    public interface IScopedMessagingService
    {
        Task<TResponse> RequestAsync<TRequest, TResponse>(string queue, TRequest message)
            where TRequest : class
            where TResponse : class;
    }
}
