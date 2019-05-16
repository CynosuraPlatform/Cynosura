using System;
using System.Threading.Tasks;
using MassTransit;

namespace Cynosura.Messaging
{
    public class RabbitContextAdvancedConsumer<T> : IConsumer<T> where T : class
    {
        private readonly Func<T, ConsumeContext<T>, Task> _queue;
        private readonly Action _done;

        public RabbitContextAdvancedConsumer(Func<T, ConsumeContext<T>, Task> queue, Action done = null)
        {
            _queue = queue;
            _done = done;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            try
            {
                await _queue(context.Message, context);
            }
            catch (Exception)
            {
                // nothing
            }
            finally
            {
                _done?.Invoke();
            }
        }
    }
}
