using Cynosura.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cynosura.Web.UnitTests
{
    public class ConfigurationProviderTests
    {
        //[Fact]
        public void ConfigureSuccess()
        {
            var provider = new ConfigurationProvider<TestConfig>();
            var config = new TestConfig();
            provider.Configure(config);
            Assert.Single(config.Values);
        }

    }

    public class TestConfig
    {
        public List<string> Values { get; } = new List<string>();
    }

    public class TestModule : IConfigurationModule<TestConfig>
    {
        public void Configure(TestConfig configuration)
        {
            configuration.Values.Add("TestModule");
        }
    }
}
