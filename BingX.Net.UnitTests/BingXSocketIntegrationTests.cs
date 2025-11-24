using BingX.Net.Clients;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BingX.Net.UnitTests
{
    [NonParallelizable]
    internal class BingXSocketIntegrationTests : SocketIntegrationTest<BingXSocketClient>
    {
        public override bool Run { get; set; } = true;

        public BingXSocketIntegrationTests()
        {
        }

        public override BingXSocketClient GetClient(ILoggerFactory loggerFactory, bool useNewDeserialization)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BingXSocketClient(Options.Create(new BingXSocketOptions
            {
                OutputOriginalData = true,
                UseUpdatedDeserialization = useNewDeserialization,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }), loggerFactory);
        }

        private BingXRestClient GetRestClient(bool useNewDeserialization)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BingXRestClient(x =>
            {
                x.UseUpdatedDeserialization = useNewDeserialization;
                x.ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null;
            });
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestSubscriptions(bool useNewDeserialization)
        {
            var listenKey = await GetRestClient(useNewDeserialization).SpotApi.Account.StartUserStreamAsync();
            await RunAndCheckUpdate<BingXTickerUpdate>(useNewDeserialization, (client, updateHandler) => client.SpotApi.SubscribeToBalanceUpdatesAsync(listenKey.Data, default, default), false, true);
            await RunAndCheckUpdate<BingXTickerUpdate>(useNewDeserialization, (client, updateHandler) => client.SpotApi.SubscribeToTickerUpdatesAsync("ETH-USDT", updateHandler, default), true, false);

            listenKey = await GetRestClient(useNewDeserialization).PerpetualFuturesApi.Account.StartUserStreamAsync();
            await RunAndCheckUpdate<BingXTickerUpdate>(useNewDeserialization, (client, updateHandler) => client.PerpetualFuturesApi.SubscribeToUserDataUpdatesAsync(listenKey.Data, default, default, default, default, default), false, true);
            await RunAndCheckUpdate<BingXFuturesTickerUpdate>(useNewDeserialization, (client, updateHandler) => client.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", updateHandler, default), true, false);
        } 
    }
}
