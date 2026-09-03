using BingX.Net.Clients.SpotApi;
using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXRestClientPerpetualFuturesSharedApi : 
        SharedApiBase,
        IBingXRestClientPerpetualFuturesApiShared,
        IBingXRestClientPerpetualFuturesSharedApi
    {
        private readonly BingXRestClientPerpetualFuturesApi _api;

        private const string _exchangeName = "BingX";
        private const string _topicId = "BingXPerpFutures";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BingXExchange.Metadata, this);
        
        public BingXRestClientPerpetualFuturesSharedApi(BingXRestClientPerpetualFuturesApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  new[] { TradingMode.PerpetualLinear },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetKlinesOptions,
                GetFuturesSymbolsOptions,
                GetFuturesTickerOptions,
                GetAllFuturesTickersOptions,
                GetBookTickerOptions,
                GetRecentTradesOptions,
                GetLeverageOptions,
                GetOrderBookOptions,
                GetIndexPriceKlinesOptions,
                GetMarkPriceKlinesOptions,
                GetOpenInterestOptions,
                GetFundingRateHistoryOptions,
                PlaceFuturesOrderOptions,
                GetFuturesOrderOptions,
                GetOpenFuturesOrdersOptions,
                GetClosedFuturesOrdersOptions,
                GetFuturesOrderTradesOptions,
                GetFuturesUserTradeHistoryOptions,
                CancelFuturesOrderOptions,
                GetPositionsOptions,
                ClosePositionOptions,
                GetFuturesOrderByClientOrderIdOptions,
                CancelFuturesOrderByClientOrderIdOptions,
                GetBalancesOptions,
                GetPositionModeOptions,
                SetPositionModeOptions,
                GetPositionHistoryOptions,
                GetFeeOptions,
                SetFuturesTpSlOptions,
                CancelFuturesTpSlOptions,
                SetLeverageOptions
                );
        }
    }
}
