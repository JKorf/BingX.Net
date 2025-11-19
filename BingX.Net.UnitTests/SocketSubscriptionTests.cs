using BingX.Net.Clients;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [TestCase(false)]
        [TestCase(true)]
        public async Task ValidateSpotSubscriptions(bool newDeserialization)
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new BingXSocketClient(Options.Create(new BingXSocketOptions
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456"),
                UseUpdatedDeserialization = newDeserialization
            }), logger);
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
