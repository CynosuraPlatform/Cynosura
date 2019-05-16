using System;
using System.Threading.Tasks;
using MassTransit;

namespace Cynosura.Messaging
{
    public class FuncWrapperConsumer<T> : IConsumer<T> where T : class
    {
        private readonly Func<T, ConsumeContext<T>, Task> _consume;
        private readonly Action _done;

        public FuncWrapperConsumer(Func<T, ConsumeContext<T>, Task> consume, Action done = null)
        {
            _consume = consume;
            _done = done;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            try
            {
                await _consume(context.Message, context);
            }
            finally
            {
                _done?.Invoke();
            }
        }
    }
}
