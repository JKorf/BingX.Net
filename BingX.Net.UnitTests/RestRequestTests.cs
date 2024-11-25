using NUnit.Framework;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using BingX.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using System.Collections.Generic;
using BingX.Net.Objects.Models;
using System.Drawing;
using BingX.Net.Enums;
using System;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateSpotAccountCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
                opts.OutputOriginalData = true;
            });
            var tester = new RestRequestValidator<BingXRestClient>(client, "Endpoints/Spot/Account", "https://open-api.bingx.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetBalancesAsync(), "GetBalances", nestedJsonProperty: "data.balances");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositHistoryAsync(), "GetDepositHistory", ignoreProperties: new List<string> { "status" });
            await tester.ValidateAsync(client => client.SpotApi.Account.GetWithdrawalHistoryAsync(), "GetWithdrawalHistory", ignoreProperties: new List<string> { "status" });
            await tester.ValidateAsync(client => client.SpotApi.Account.GetAssetsAsync(), "GetAssets", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.WithdrawAsync("ETH", "123", 1, Enums.AccountType.Funding), "Withdraw", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositAddressAsync("ETH"), "GetDepositAddress", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.TransferAsync(Enums.TransferType.PerpetualFuturesToStandardFutures, "ETH", 1), "Transfer");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetTransfersAsync(Enums.TransferType.PerpetualFuturesToStandardFutures), "GetTransfers");
            await tester.ValidateAsync(client => client.SpotApi.Account.TransferInternalAsync("ETH", Enums.AccountIdentifierType.Uid, "123", 1, Enums.AccountType.Perpetual), "TransferInternal", "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetInternalTransfersAsync("ETH"), "GetInternalTransfers", "data", new List<string> { "status" });
            await tester.ValidateAsync(client => client.SpotApi.Account.StartUserStreamAsync(), "StartUserStream", "listenKey");
            await tester.ValidateAsync(client => client.SpotApi.Account.KeepAliveUserStreamAsync("123"), "KeepAliveUserStream");
            await tester.ValidateAsync(client => client.SpotApi.Account.StopUserStreamAsync("123"), "StopUserStream");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetTradingFeesAsync("ETH"), "GetTradingFees", "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetUserIdAsync(), "GetUserId", "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetApiKeyPermissionsAsync(123), "GetApiKeyPermissions", "data.apiInfos", ignoreProperties: new List<string> { "permissions" });
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BingXRestClient>(client, "Endpoints/Spot/ExchangeData", "https://open-api.bingx.com", IsAuthenticated, "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data.symbols");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETHUSDT"), "GetRecentTrades");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETHUSDT"), "GetOrderBook");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetAggregatedOrderBookAsync("ETHUSDT", 20, 0), "GetAggregatedOrderBook");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.KlineInterval.OneDay), "GetKlines");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync("ETHUSDT"), "GetTickers");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetBookPriceAsync("ETHUSDT"), "GetBookPrice", useSingleArrayItem: true);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT"), "GetTradeHistory");
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BingXRestClient>(client, "Endpoints/Spot/Trading", "https://open-api.bingx.com", IsAuthenticated, "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceOrderAsync("ETHUSDT", Enums.OrderSide.Buy, Enums.OrderType.Market, 1), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceMultipleOrdersAsync(new[] { new BingXPlaceOrderRequest() }), "PlaceMultipleOrders", nestedJsonProperty: "data.orders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOrderAsync("ETHUSDT", 123), "CancelOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOrdersAsync("ETHUSDT", new[] { 123L }), "CancelOrders", nestedJsonProperty: "data.orders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelAllOrdersAsync("ETHUSDT"), "CancelAllOrders", nestedJsonProperty: "data.orders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelAllOrdersAfterAsync(true, 1), "CancelAllOrdersAfter", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderAsync("ETHUSDT", 123), "GetOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOpenOrdersAsync("ETHUSDT"), "GetOpenOrders", nestedJsonProperty: "data.orders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrdersAsync("ETHUSDT"), "GetOrders", nestedJsonProperty: "data.orders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetUserTradesAsync("ETHUSDT"), "GetUserTrades", nestedJsonProperty: "data.fills");
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceOcoOrderAsync("123", Enums.OrderSide.Buy, 0.1m, 0.1m, 0.1m, 0.1m), "PlaceOcoOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOcoOrderAsync("123"), "CancelOcoOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOcoOrderAsync("123"), "GetOcoOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOpenOcoOrdersAsync(123, 123), "GetOpenOcoOrders", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidatePerpetualFuturesAccountCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BingXRestClient>(client, "Endpoints/PerpetualFutures/Account", "https://open-api.bingx.com", IsAuthenticated, "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetBalancesAsync(), "GetBalances", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetIncomesAsync(), "GetIncomes");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetTradingFeesAsync(), "GetTradingFees", "data.commission");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.StartUserStreamAsync(), "StartUserStream", "listenKey");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.KeepAliveUserStreamAsync("123"), "KeepAliveUserStream");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.StopUserStreamAsync("123"), "StopUserStream");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetMarginModeAsync("ETHUDST"), "GetMarginMode");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.SetMarginModeAsync("ETHUDST", Enums.MarginMode.Isolated), "SetMarginMode");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetLeverageAsync("ETHUDST"), "GetLeverage");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.SetLeverageAsync("ETHUDST", Enums.PositionSide.Short, 10), "SetLeverage");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.AdjustIsolatedMarginAsync("ETHUDST", 1, Enums.AdjustDirection.Decrease, Enums.PositionSide.Long), "AdjustIsolatedMargin");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetPositionModeAsync(), "GetPositionMode");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.SetPositionModeAsync(Enums.PositionMode.SinglePositionMode), "SetPositionMode");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetIsolatedMarginChangeHistoryAsync("123"), "GetIsolatedMarginChangeHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.SetMultiAssetModeAsync(Enums.MultiAssetMode.SingleAssetMode), "SetMultiAssetMode", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetMultiAssetModeAsync(), "GetMultiAssetMode", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetMultiAssetRulesAsync(), "GetMultiAssetRules", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.GetMultiAssetsMarginAsync(), "GetMultiAssetsMargin", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Account.ApplyForVSTAssetsAsync(), "ApplyForVSTAssets", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidatePerpetualFuturesExchangeDataCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BingXRestClient>(client, "Endpoints/PerpetualFutures/ExchangeData", "https://open-api.bingx.com", IsAuthenticated, "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetContractsAsync(), "GetContracts", nestedJsonProperty: "data", ignoreProperties: new List<string> { "tradeMinLimit" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetOrderBookAsync("ETHUSDT"), "GetOrderBook", nestedJsonProperty: "data", ignoreProperties: new List<string> { "T" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetRecentTradesAsync("ETHUSDT"), "GetRecentTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT"), "GetTradeHistory", nestedJsonProperty: "data", ignoreProperties: new List<string> { "id" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetFundingRateAsync("ETHUSDT"), "GetFundingRate", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetFundingRateHistoryAsync("ETHUSDT"), "GetFundingRateHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.KlineInterval.OneWeek), "GetKlines", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetMarkPriceKlinesAsync("ETHUSDT", Enums.KlineInterval.OneWeek), "GetMarkPriceKlines", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetOpenInterestAsync("ETHUSDT"), "GetOpenInterest", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetTickerAsync("ETHUSDT"), "GetTicker", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetBookTickerAsync("ETHUSDT"), "GetBookTicker", nestedJsonProperty: "data.book_ticker");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.ExchangeData.GetLastTradePriceAsync("ETHUSDT"), "GetLastTradePrice", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidatePerpetualFuturesTradingCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BingXRestClient>(client, "Endpoints/PerpetualFutures/Trading", "https://open-api.bingx.com", IsAuthenticated, "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetPositionsAsync(), "GetPositions");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.PlaceTestOrderAsync("ETHUSDT", Enums.OrderSide.Buy, Enums.FuturesOrderType.Market, Enums.PositionSide.Long, 1), "PlaceTestOrder", "data.order", ignoreProperties: new List<string> { "workingType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.PlaceOrderAsync("ETHUSDT", Enums.OrderSide.Buy, Enums.FuturesOrderType.Market, Enums.PositionSide.Long, 1), "PlaceOrder", "data.order", ignoreProperties: new List<string> { "workingType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.PlaceMultipleOrderAsync(new[] { new BingXFuturesPlaceOrderRequest() }), "PlaceMultipleOrder", "data.orders", ignoreProperties: new List<string> { "workingType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetOrderAsync("ETHUSDT", 1), "GetOrder", "data.order", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.CancelOrderAsync("ETHUSDT", 1), "CancelOrder", "data.order", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.CloseAllPositionsAsync("ETHUSDT"), "CloseAllPositions");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.CancelMultipleOrderAsync("ETHUSDT", new[] { 1L }), "CancelMultipleOrder", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.CancelAllOrderAsync("ETHUSDT"), "CancelAllOrder", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetOpenOrdersAsync("ETHUSDT"), "GetOpenOrders", "data.orders", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetLiquidationOrdersAsync("ETHUSDT"), "GetLiquidationOrders", "data.orders", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetClosedOrdersAsync("ETHUSDT"), "GetClosedOrders", "data.orders", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType", "type" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetUserTradesAsync(), "GetUserTrades", "data.fill_orders", ignoreProperties: new List<string> { "filledTime" });
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.CancelAllOrdersAfterAsync(true, 1), "CancelAllOrdersAfter");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.ClosePositionAsync("123"), "ClosePosition");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetPositionAndMarginInfoAsync("123"), "GetPositionAndMarginInfo");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetPositionHistoryAsync("ETH-USDT"), "GetPositionHistory", nestedJsonProperty: "data.positionHistory");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.PlaceTwapOrderAsync("123", OrderSide.Sell, PositionSide.Long, PriceType.Constant, 123, 0.1m, 123, 0.1m, 0.1m), "PlaceTwapOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetClosedTwapOrdersAsync("123", 123, 123, DateTime.UtcNow, DateTime.UtcNow), "GetClosedTwapOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetTwapOrderAsync(123), "GetTwapOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.CancelTwapOrderAsync(123), "CancelTwapOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PerpetualFuturesApi.Trading.GetOrdersAsync("123", 123), "GetOrders", nestedJsonProperty: "data.orders", ignoreProperties: new List<string> { "workingType", "advanceAttr", "positionID", "takeProfitEntrustPrice", "stopLossEntrustPrice", "orderType" });
        }


        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl.Contains("signature") || result.RequestBody?.Contains("signature") == true;
        }
    }
}
