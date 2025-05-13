using NUnit.Framework;
using System.Threading.Tasks;
using BingX.Net.Clients;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Testing;
using System.Collections.Generic;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotSubscriptions()
        {
            var client = new BingXSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<BingXSocketClient>(client, "Subscriptions/Spot", "wss://open-api-ws.bingx.com/market");
            await tester.ValidateAsync<BingXTradeUpdate>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("BTC-USDT", handler), "Trades", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXKlineUpdate>((client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("BTC-USDT", Enums.KlineInterval.TwoHours, handler), "Klines", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXOrderBook>((client, handler) => client.SpotApi.SubscribeToPartialOrderBookUpdatesAsync("BTC-USDT", 20, handler), "PartialBook", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXTickerUpdate>((client, handler) => client.SpotApi.SubscribeToTickerUpdatesAsync("BTC-USDT", handler), "Ticker", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXPriceUpdate>((client, handler) => client.SpotApi.SubscribeToPriceUpdatesAsync("BTC-USDT", handler), "Price", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXBookTickerUpdate>((client, handler) => client.SpotApi.SubscribeToBookPriceUpdatesAsync("BTC-USDT", handler), "BookPrice", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXOrderUpdate>((client, handler) => client.SpotApi.SubscribeToOrderUpdatesAsync("123", handler), "Order", ignoreProperties: new List<string> { "m" }, nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXBalanceUpdate>((client, handler) => client.SpotApi.SubscribeToBalanceUpdatesAsync("123", handler), "Balance");
        }

        [Test]
        public async Task ValidatePerpetualFutureSubscriptions()
        {
            var client = new BingXSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<BingXSocketClient>(client, "Subscriptions/PerpetualFutures", "wss://open-api-ws.bingx.com/market");
            await tester.ValidateAsync<BingXFuturesTradeUpdate[]>((client, handler) => client.PerpetualFuturesApi.SubscribeToTradeUpdatesAsync("ETH-USDT", handler), "Trades", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXOrderBook>((client, handler) => client.PerpetualFuturesApi.SubscribeToPartialOrderBookUpdatesAsync("ETH-USDT", 20, 100, handler), "PartialBook", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXFuturesKlineUpdate[]>((client, handler) => client.PerpetualFuturesApi.SubscribeToKlineUpdatesAsync("ETH-USDT", Enums.KlineInterval.OneMinute, handler), "Klines", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXFuturesTickerUpdate>((client, handler) => client.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", handler), "Ticker", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXPriceUpdate>((client, handler) => client.PerpetualFuturesApi.SubscribeToPriceUpdatesAsync("ETH-USDT", handler), "Price", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXMarkPriceUpdate>((client, handler) => client.PerpetualFuturesApi.SubscribeToMarkPriceUpdatesAsync("ETH-USDT", handler), "MarkPrice", nestedJsonProperty: "data");
            await tester.ValidateAsync<BingXBookTickerUpdate>((client, handler) => client.PerpetualFuturesApi.SubscribeToBookPriceUpdatesAsync("ETH-USDT", handler), "BookPrice", nestedJsonProperty: "data");
        }
    }
}
