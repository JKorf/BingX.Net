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
        public override bool Run { get; set; }

        public BingXSocketIntegrationTests()
        {
        }

        public override BingXSocketClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BingXSocketClient(Options.Create(new BingXSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }), loggerFactory);
        }

        private BingXRestClient GetRestClient()
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BingXRestClient(x =>
            {
                x.ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null;
            });
        }

        [Test]
        public async Task TestSubscriptions()
        {
            var listenKey = await GetRestClient().SpotApi.Account.StartUserStreamAsync();
            await RunAndCheckUpdate<BingXTickerUpdate>((client, updateHandler) => client.SpotApi.SubscribeToBalanceUpdatesAsync(listenKey.Data, default, default), false, true);
            await RunAndCheckUpdate<BingXTickerUpdate>((client, updateHandler) => client.SpotApi.SubscribeToTickerUpdatesAsync("ETH-USDT", updateHandler, default), true, false);

            listenKey = await GetRestClient().PerpetualFuturesApi.Account.StartUserStreamAsync();
            await RunAndCheckUpdate<BingXTickerUpdate>((client, updateHandler) => client.PerpetualFuturesApi.SubscribeToUserDataUpdatesAsync(listenKey.Data, default, default, default, default, default), false, true);
            await RunAndCheckUpdate<BingXFuturesTickerUpdate>((client, updateHandler) => client.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", updateHandler, default), true, false);
        } 
    }
}
