using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Authentication;
using BingX.Net.Interfaces.Clients;
using BingX.Net.UnitTests.TestImplementations;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class JsonTests
    {
        private JsonToObjectComparer<IBingXRestClient> _comparer = new JsonToObjectComparer<IBingXRestClient>((json) => TestHelpers.CreateResponseClient(json, options =>
        {
            options.ApiCredentials = new ApiCredentials("123", "123");
            options.RateLimiterEnabled = false;
            options.SpotOptions.AutoTimestamp = false;
            options.FuturesOptions.AutoTimestamp = false;
        }));

        [Test]
        public async Task ValidateSpotAccountCalls()
        {
            await _comparer.ProcessSubject(
                "Spot/Account",
                c => c.SpotApi.Account);
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            await _comparer.ProcessSubject(
                "Spot/ExchangeData",
                c => c.SpotApi.ExchangeData, 
                useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "GetSymbolsAsync", "data:symbols" },
                    { "GetAggregatedOrderBookAsync", "data" },
                    { "GetRecentTradesAsync", "data" },
                    { "GetBookPriceAsync", "data" },
                    { "GetKlinesAsync", "data" },
                    { "GetOrderBookAsync", "data" },
                    { "GetTickersAsync", "data" },
                    { "GetTradeHistoryAsync", "data" },
                },
                useFirstItemInArray: new List<string>
                {
                    "GetBookPriceAsync"
                });
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            await _comparer.ProcessSubject(
                "Spot/Trading",
                c => c.SpotApi.Trading,
                 useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "PlaceOrderAsync", "data" },
                    { "CancelOrderAsync", "data" },
                    { "GetOrderAsync", "data" },
                    { "GetOpenOrdersAsync", "data:orders" },
                    { "GetUserTradesAsync", "data:fills" },
                 });
        }

        [Test]
        public async Task ValidateFuturesAccountCalls()
        {
            await _comparer.ProcessSubject(
                "Futures/Account",
                c => c.PerpetualFuturesApi.Account,
                 useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "GetBalancesAsync", "data:balance" },
                    { "GetIncomesAsync", "data" },
                    { "GetTradingFeesAsync", "data:commission" },
                 });
        }

        [Test]
        public async Task ValidateFuturesExchangeDataCalls()
        {
            await _comparer.ProcessSubject(
                "Futures/ExchangeData",
                c => c.PerpetualFuturesApi.ExchangeData,
                 useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                 });
        }

        [Test]
        public async Task ValidateFuturesTradingCalls()
        {
            await _comparer.ProcessSubject(
                "Futures/Trading",
                c => c.PerpetualFuturesApi.Trading,
                 useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "GetPositionsAsync", "data" },
                    { "PlaceOrderAsync", "data:order" },
                    { "GetOrderAsync", "data:order" },
                    { "GetUserTradesAsync", "data:fill_orders" },
                    { "GetLiquidationOrdersAsync", "data:orders" },
                 },
                 ignoreProperties: new Dictionary<string, List<string>>
                 {
                     { "PlaceOrderAsync", new List<string> { "takeProfit", "stopLoss" } }, // Just returns the input
                     { "GetOrderAsync", new List<string> { "advanceAttr", "triggerOrderId", "onlyOnePosition", "stopLossEntrustPrice" , "takeProfitEntrustPrice", "positionID", "orderType" } }, // Not described in API documentation, not clear what they mean
                     { "GetLiquidationOrdersAsync", new List<string> { "advanceAttr", "triggerOrderId", "onlyOnePosition", "stopLossEntrustPrice", "takeProfitEntrustPrice", "positionID", "orderType" } }, // Not described in API documentation, not clear what they mean
                     { "GetUserTradesAsync", new List<string> { "filledTime" } }, // Just returns the input
                 });
        }
    }
}
