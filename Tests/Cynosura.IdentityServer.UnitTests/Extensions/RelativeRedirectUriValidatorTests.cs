using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Cynosura.IdentityServer;
using Moq;
using Duende.IdentityServer.Models;

namespace Cynosura.IdentityServer.UnitTests.Extensions
{
    public class RelativeRedirectUriValidatorTests
    {
        [Theory]
        [InlineData("https://example.com", "https://example.com")]
        [InlineData("https://*.example.com", "https://test.example.com")]
        public async Task IsRedirectUriValidAsync_Success(string uri, string requestedUri)
        {
            var absoluteUrlFactory = new Mock<IAbsoluteUrlFactory>();
            var relativeRedirectUriValidator = new RelativeRedirectUriValidator(absoluteUrlFactory.Object);
            var client = new Client()
            {
                RedirectUris = new List<string>() { uri }
            };

            var result = await relativeRedirectUriValidator.IsRedirectUriValidAsync(requestedUri, client);

            Assert.True(result);
        }

        [Theory]
        [InlineData("https://example.com", "https://example2.com")]
        [InlineData("https://*.example.com", "https://test.example2.com")]
        public async Task IsRedirectUriValidAsync_Fail(string uri, string requestedUri)
        {
            var absoluteUrlFactory = new Mock<IAbsoluteUrlFactory>();
            var relativeRedirectUriValidator = new RelativeRedirectUriValidator(absoluteUrlFactory.Object);
            var client = new Client()
            {
                RedirectUris = new List<string>() { uri }
            };

            var result = await relativeRedirectUriValidator.IsRedirectUriValidAsync(requestedUri, client);

            Assert.False(result);
        }
    }
}
