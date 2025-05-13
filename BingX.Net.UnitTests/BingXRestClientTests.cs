using NUnit.Framework;
using BingX.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CryptoExchange.Net.Objects;
using BingX.Net.Interfaces.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace BingX.Net.UnitTests
{
    [TestFixture()]
    public class BingXRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new BingXAuthenticationProvider(new ApiCredentials("hO6oQotzTE0S5FRYze2Jx2wGx7eVnJGMolpA1nZyehsoMgCcgKNWQHd4QgTFZuwl4Zt4xMe2PqGBegWXO4A", "mheO6dR8ovSsxZQCOYEFCtelpuxcWGTfHw7te326y6jOwq5WpvFQ9JNljoTwBXZGv5It07m9RXSPpDQEK2w"));
            var client = (RestApiClient)new BingXRestClient().SpotApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "",
                (uriParams, bodyParams, headers) =>
                {
                    return bodyParams["signature"].ToString();
                },
                "8D0D3EA9B592BE3678C33332AB13E9102E093E67255921E15A581146C87C272F",
                new Dictionary<string, object>
                {
                    { "recvWindow", 0 },
                    { "subAccountString", "abc12345" },
                },
                DateTimeConverter.ParseFromDouble(1696751141337),
                true,
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<BingXRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<BingXSocketClient>();
        }


        [Test]
        [TestCase(TradeEnvironmentNames.Live, "https://open-api.bingx.com")]
        [TestCase("", "https://open-api.bingx.com")]
        public void TestConstructorEnvironments(string environmentName, string expected)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "BingX:Environment:Name", environmentName },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddBingX(configuration.GetSection("BingX"));
            var provider = collection.BuildServiceProvider();

            var client = provider.GetRequiredService<IBingXRestClient>();

            var address = client.SpotApi.BaseAddress;

            Assert.That(address, Is.EqualTo(expected));
        }

        [Test]
        public void TestConstructorNullEnvironment()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "BingX", null },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddBingX(configuration.GetSection("BingX"));
            var provider = collection.BuildServiceProvider();

            var client = provider.GetRequiredService<IBingXRestClient>();

            var address = client.SpotApi.BaseAddress;

            Assert.That(address, Is.EqualTo("https://open-api.bingx.com"));
        }

        [Test]
        public void TestConstructorApiOverwriteEnvironment()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "BingX:Environment:Name", "test" },
                    { "BingX:Rest:Environment:Name", "live" },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddBingX(configuration.GetSection("BingX"));
            var provider = collection.BuildServiceProvider();

            var client = provider.GetRequiredService<IBingXRestClient>();

            var address = client.SpotApi.BaseAddress;

            Assert.That(address, Is.EqualTo("https://open-api.bingx.com"));
        }

        [Test]
        public void TestConstructorConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ApiCredentials:Key", "123" },
                    { "ApiCredentials:Secret", "456" },
                    { "Socket:ApiCredentials:Key", "456" },
                    { "Socket:ApiCredentials:Secret", "789" },
                    { "Rest:OutputOriginalData", "true" },
                    { "Socket:OutputOriginalData", "false" },
                    { "Rest:Proxy:Host", "host" },
                    { "Rest:Proxy:Port", "80" },
                    { "Socket:Proxy:Host", "host2" },
                    { "Socket:Proxy:Port", "81" },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddBingX(configuration);
            var provider = collection.BuildServiceProvider();

            var restClient = provider.GetRequiredService<IBingXRestClient>();
            var socketClient = provider.GetRequiredService<IBingXSocketClient>();

            Assert.That(((BaseApiClient)restClient.SpotApi).OutputOriginalData, Is.True);
            Assert.That(((BaseApiClient)socketClient.SpotApi).OutputOriginalData, Is.False);
            Assert.That(((BaseApiClient)restClient.SpotApi).AuthenticationProvider.ApiKey, Is.EqualTo("123"));
            Assert.That(((BaseApiClient)socketClient.SpotApi).AuthenticationProvider.ApiKey, Is.EqualTo("456"));
            Assert.That(((BaseApiClient)restClient.SpotApi).ClientOptions.Proxy.Host, Is.EqualTo("host"));
            Assert.That(((BaseApiClient)restClient.SpotApi).ClientOptions.Proxy.Port, Is.EqualTo(80));
            Assert.That(((BaseApiClient)socketClient.SpotApi).ClientOptions.Proxy.Host, Is.EqualTo("host2"));
            Assert.That(((BaseApiClient)socketClient.SpotApi).ClientOptions.Proxy.Port, Is.EqualTo(81));
        }
    }
}
