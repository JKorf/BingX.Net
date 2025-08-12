using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXSocketClientPerpetualFuturesApi : IBingXSocketClientPerpetualFuturesApiShared
    {
        private const string _topicId = "BingXPerpFutures";
        public string Exchange => BingXExchange.ExchangeName;

        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.PerpetualLinear };
        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Ticker client
        EndpointOptions<SubscribeTickerRequest> ITickerSocketClient.SubscribeTickerOptions { get; } = new EndpointOptions<SubscribeTickerRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<ExchangeEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = ((ITickerSocketClient)this).SubscribeTickerOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToTickerUpdatesAsync(symbol, update => handler(update.AsExchangeEvent(Exchange, new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol), update.Data.Symbol, update.Data.LastPrice, update.Data.HighPrice, update.Data.LowPrice, update.Data.Volume, update.Data.PriceChangePercentage)
            {
                QuoteVolume = update.Data.QuoteVolume
            })), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Trade client

        EndpointOptions<SubscribeTradeRequest> ITradeSocketClient.SubscribeTradeOptions { get; } = new EndpointOptions<SubscribeTradeRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<ExchangeEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = ((ITradeSocketClient)this).SubscribeTradeOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToTradeUpdatesAsync(symbol, update => handler(update.AsExchangeEvent<SharedTrade[]>(Exchange, update.Data.Select(x => new SharedTrade(x.Quantity, x.Price, x.TradeTime)
            {
                Side = x.BuyerIsMaker ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray())), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Kline client
        SubscribeKlineOptions IKlineSocketClient.SubscribeKlineOptions { get; } = new SubscribeKlineOptions(false);
        async Task<ExchangeResult<UpdateSubscription>> IKlineSocketClient.SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<ExchangeEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeResult<UpdateSubscription>(Exchange, ArgumentError.Invalid(nameof(SubscribeKlineRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineSocketClient)this).SubscribeKlineOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToKlineUpdatesAsync(symbol, interval, update =>
            {
                if (update.UpdateType == SocketUpdateType.Snapshot)
                    return;
                
                foreach (var item in update.Data)
                    handler(update.AsExchangeEvent(Exchange, new SharedKline(item.Timestamp, item.ClosePrice, item.HighPrice, item.LowPrice, item.OpenPrice, item.Volume)));
            }, ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
        #endregion

        #region Book Ticker client

        EndpointOptions<SubscribeBookTickerRequest> IBookTickerSocketClient.SubscribeBookTickerOptions { get; } = new EndpointOptions<SubscribeBookTickerRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<ExchangeEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = ((IBookTickerSocketClient)this).SubscribeBookTickerOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToBookPriceUpdatesAsync(symbol, update => handler(update.AsExchangeEvent(Exchange, new SharedBookTicker(ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol), update.Data.Symbol, update.Data.BestAskPrice, update.Data.BestAskQuantity, update.Data.BestBidPrice, update.Data.BestBidQuantity))), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Balance client
        EndpointOptions<SubscribeBalancesRequest> IBalanceSocketClient.SubscribeBalanceOptions { get; } = new EndpointOptions<SubscribeBalancesRequest>(false)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SubscribeBalancesRequest.ListenKey), typeof(string), "The listenkey for starting the user stream", "123123123")
            }
        };
        async Task<ExchangeResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<ExchangeEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = ((IBalanceSocketClient)this).SubscribeBalanceOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(request.ListenKey!,
                onAccountUpdate: update => handler(update.AsExchangeEvent<SharedBalance[]>(Exchange, update.Data.Update.Balances.Select(x => new SharedBalance(x.Asset, x.BalanceExIsolatedMargin, x.Balance)).ToArray())),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Futures Order client
        EndpointOptions<SubscribeFuturesOrderRequest> IFuturesOrderSocketClient.SubscribeFuturesOrderOptions { get; } = new EndpointOptions<SubscribeFuturesOrderRequest>(false)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SubscribeFuturesOrderRequest.ListenKey), typeof(string), "The listenkey for starting the user stream", "123123123")
            }
        };
        async Task<ExchangeResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<ExchangeEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderSocketClient)this).SubscribeFuturesOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(request.ListenKey!,
                onOrderUpdate: update => handler(update.AsExchangeEvent<SharedFuturesOrder[]>(Exchange, new[] {
                    new SharedFuturesOrder(
                        ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        ParseOrderType(update.Data),
                        update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        update.Data.Status == Enums.OrderStatus.Canceled ? SharedOrderStatus.Canceled : (update.Data.Status == Enums.OrderStatus.New || update.Data.Status == Enums.OrderStatus.PartiallyFilled) ? SharedOrderStatus.Open : SharedOrderStatus.Filled,
                        update.Data.UpdateTime)
                    {
                        ClientOrderId = update.Data.ClientOrderId,
                        OrderPrice = update.Data.Price,
                        OrderQuantity = new SharedOrderQuantity(update.Data.Quantity, null, update.Data.Quantity),
                        QuantityFilled = new SharedOrderQuantity(update.Data.QuantityFilled, update.Data.VolumeFilled, update.Data.QuantityFilled),
                        Fee = update.Data.Fee == null ? null :Math.Abs(update.Data.Fee.Value),
                        AveragePrice = update.Data.AveragePrice == 0 ? null : update.Data.AveragePrice,
                        PositionSide = update.Data.PositionSide == Enums.PositionSide.Long ? SharedPositionSide.Long : update.Data.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : null,
                        FeeAsset = update.Data.FeeAsset,
                        UpdateTime = update.Data.UpdateTime,
                        ReduceOnly = update.Data.ReduceOnly,
                        TriggerPrice = update.Data.TriggerPrice,
                        IsTriggerOrder = update.Data.TriggerPrice > 0,
                        IsCloseOrder = (update.Data.Type == Enums.FuturesOrderType.TakeProfitMarket || update.Data.Type == Enums.FuturesOrderType.TakeProfitLimit || update.Data.Type == Enums.FuturesOrderType.StopLimit || update.Data.Type == Enums.FuturesOrderType.StopMarket)
                                            && (update.Data.Quantity == null || update.Data.Quantity == 0)
                    }
                })),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        private SharedOrderType ParseOrderType(BingXFuturesOrderUpdate data)
        {
            if (data.Type == Enums.FuturesOrderType.Market
                || data.Type == Enums.FuturesOrderType.StopMarket
                || data.Type == Enums.FuturesOrderType.TriggerMarket
                || data.Type == Enums.FuturesOrderType.TakeProfitMarket)
            {
                return SharedOrderType.Market;
            }

            if (data.Type == Enums.FuturesOrderType.Limit
                || data.Type == Enums.FuturesOrderType.StopLimit
                || data.Type == Enums.FuturesOrderType.TakeProfitLimit)
            {
                return SharedOrderType.Limit;
            }

            return SharedOrderType.Other;
        }

        #endregion

        #region Position client
        EndpointOptions<SubscribePositionRequest> IPositionSocketClient.SubscribePositionOptions { get; } = new EndpointOptions<SubscribePositionRequest>(false)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SubscribePositionRequest.ListenKey), typeof(string), "The listenkey for starting the user stream", "123123123")
            }
        };
        async Task<ExchangeResult<UpdateSubscription>> IPositionSocketClient.SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<ExchangeEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = ((IPositionSocketClient)this).SubscribePositionOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(request.ListenKey!,
                onAccountUpdate: update => handler(update.AsExchangeEvent<SharedPosition[]>(Exchange, update.Data.Update.Positions.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, x.Size, update.Data.EventTime)
                {
                    AverageOpenPrice = x.EntryPrice,
                    PositionSide = x.Side == Enums.TradeSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                    UnrealizedPnl = x.UnrealizedPnl
                }).ToArray())),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion
    }
}
