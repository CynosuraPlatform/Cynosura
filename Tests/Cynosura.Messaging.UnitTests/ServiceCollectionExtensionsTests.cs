using Cynosura.Core.Data;
using Cynosura.Core.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cynosura.Messaging.UnitTests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCynosuraMessaging_Success()
        {
            var services = new ServiceCollection();
            var optionsMock = new Mock<IOptions<MassTransitServiceOptions>>();
            optionsMock.Setup(m => m.Value).Returns(new MassTransitServiceOptions()
            {
                ConnectionUrl = "rabbitmq://localhost",
                Username = "user",
                Password = "pass",
            });

            services.AddSingleton(optionsMock.Object);
            services.AddSingleton<ILoggerFactory, FakeLoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(FakeLogger<>));
            services.AddCynosuraMessaging();
            var provider = services.BuildServiceProvider();
            var messagingService = provider.GetService<IMessagingService>();

            Assert.NotNull(messagingService);
        }

        class FakeLoggerFactory : ILoggerFactory
        {
            public void AddProvider(ILoggerProvider provider)
            {
                throw new NotImplementedException();
            }

            public ILogger CreateLogger(string categoryName)
            {
               return new FakeLogger<string>();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        class FakeLogger<T> : ILogger<T>
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                throw new NotImplementedException();
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                throw new NotImplementedException();
            }
        }
    }
}
