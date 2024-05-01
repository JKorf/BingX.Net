using NUnit.Framework;
using System.Threading.Tasks;
using BingX.Net.Clients;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Testing;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new BingXSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<BingXSocketClient>(client, "Subscriptions/Spot/ExchangeData", "wss://open-api-ws.bingx.com/market", "data");
            await tester.ValidateAsync<BingXTradeUpdate>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("BTC-USDT", handler), "Trades");
            await tester.ValidateAsync<BingXKlineUpdate>((client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("BTC-USDT", Enums.KlineInterval.TwoHours, handler), "Klines");
            //await tester.ValidateAsync<BingXBalanceUpdate>((client, handler) => client.SpotApi.SubscribeToBalanceUpdatesAsync("123", handler), "Balance");
        }
    }
}
