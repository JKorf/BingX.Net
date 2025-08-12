using BingX.Net;
using BingX.Net.Clients;
using BingX.Net.SymbolOrderBooks;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BingX.Net.UnitTests
{
    [NonParallelizable]
    internal class BingXRestIntegrationTests : RestIntegrationTest<BingXRestClient>
    {
        public override bool Run { get; set; }

        public BingXRestIntegrationTests()
        {
            BingXExchange.RateLimiter.RateLimitTriggered += (x) => Debug.WriteLine(x);
        }

        public override BingXRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BingXRestClient(null, loggerFactory, Options.Create(new Objects.Options.BingXRestOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().SpotApi.ExchangeData.GetTickersAsync("TST-TST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorCode, Is.EqualTo("100204"));
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.UnknownSymbol));
        }

        [Test]
        public async Task TestSpotAccount()
        {
            await RunAndCheckResult(client => client.SpotApi.Account.GetBalancesAsync(default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetDepositHistoryAsync(default, default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetWithdrawalHistoryAsync(default, default, default, default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetAssetsAsync(default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetDepositAddressAsync("ETH", default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetTransfersAsync(Enums.TransferType.FundingToPerpetualFutures, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetInternalTransfersAsync("ETH", default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetTradingFeesAsync("ETH-USDT", default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetUserIdAsync(default), true);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetSymbolsAsync(default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETH-USDT", default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETH-USDT", default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetAggregatedOrderBookAsync("ETH-USDT", 5, 5, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetTickersAsync(default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetLastTradeAsync("ETH-USDT", default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetLastTradesAsync(default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetBookPriceAsync("ETH-USDT", default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETH-USDT", default, default, default), false);
        }

        [Test]
        public async Task TestSpotTrading()
        {
            await RunAndCheckResult(client => client.SpotApi.Trading.GetOpenOrdersAsync(default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Trading.GetOrdersAsync(default, default, default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Trading.GetUserTradesAsync("ETH-USDT", default, default, default, default, default, default, default, default), true);
        }

        [Test]
        public async Task TestPerpetualFuturesAccount()
        {
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Account.GetBalancesAsync(default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Account.GetIncomesAsync(default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Account.GetTradingFeesAsync(default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Account.GetMarginModeAsync("ETH-USDT", default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Account.GetLeverageAsync("ETH-USDT", default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Account.GetPositionModeAsync(default), true);
        }

        [Test]
        public async Task TestPerpetualFuturesExchangeData()
        {
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetContractsAsync(default, default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetOrderBookAsync("ETH-USDT", default, default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetRecentTradesAsync("ETH-USDT", default, default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetTradeHistoryAsync("ETH-USDT", default, default, default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetFundingRateAsync("ETH-USDT", default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetFundingRatesAsync(default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetFundingRateHistoryAsync("ETH-USDT", default, default, default, default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default, default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetMarkPriceKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default, default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetOpenInterestAsync("ETH-USDT", default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetTickerAsync("ETH-USDT", default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetTickersAsync(default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetBookTickerAsync("ETH-USDT", default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetLastTradePriceAsync("ETH-USDT", default), false);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.ExchangeData.GetLastTradePricesAsync(default), false);
        }

        [Test]
        public async Task TestPerpetualFuturesTrading()
        {
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Trading.GetPositionsAsync(default, default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Trading.GetOpenOrdersAsync(default, default, default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Trading.GetClosedOrdersAsync(default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Trading.GetLiquidationOrdersAsync(default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Trading.GetUserTradesAsync(default, default, default, default, default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Trading.GetPositionAndMarginInfoAsync("ETH-USDT", default), true);
            await RunAndCheckResult(client => client.PerpetualFuturesApi.Trading.GetPositionHistoryAsync("ADA-USDT", default, default, default, default, default, default, default), true);
        }

        [Test]
        public async Task TestOrderBooks()
        {
            await TestOrderBook(new BingXSpotSymbolOrderBook("ETH-USDT"));
            await TestOrderBook(new BingXPerpetualFuturesSymbolOrderBook("ETH-USDT"));
        }
    }
}
