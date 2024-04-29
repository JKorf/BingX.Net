using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Authentication;
using BingX.Net.Interfaces.Clients;
using BingX.Net.UnitTests.TestImplementations;
using BingX.Net.Clients;
using CryptoExchange.Test.Net;
using CryptoExchange.Net.Objects;
using BingX.Net.Objects.Models;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class SubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new BingXSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketTester<BingXSocketClient>(client, "Subscriptions/Spot/ExchangeData", "wss://open-api-ws.bingx.com/market", "data");
            await tester.ValidateAsync<BingXSocketClient, BingXTradeUpdate>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("BTC-USDT", handler), "Trades");
            await tester.ValidateAsync<BingXSocketClient, BingXKlineUpdate>((client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("BTC-USDT", Enums.KlineInterval.TwoHours, handler), "Klines");
            await tester.ValidateAsync<BingXSocketClient, BingXBalanceUpdate>((client, handler) => client.SpotApi.SubscribeToBalanceUpdatesAsync("123", handler), "Balance");

        }
    }
}
