using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXSocketClientSpotApi : IBingXSocketClientSpotApiShared
    {
        private const string _topicId = "BingXSpot";
        public string Exchange => BingXExchange.ExchangeName;

        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.Spot };
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
            var result = await SubscribeToTickerUpdatesAsync(symbol, update => handler(update.AsExchangeEvent(Exchange, new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol), update.Data.Symbol, update.Data.LastPrice, update.Data.HighPrice, update.Data.LowPrice, update.Data.Volume, decimal.Parse(update.Data.PriceChangePercentage.Substring(0, update.Data.PriceChangePercentage.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture))
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
            var result = await SubscribeToTradeUpdatesAsync(symbol, update => handler(update.AsExchangeEvent<SharedTrade[]>(Exchange, new[] { new SharedTrade(update.Data.Quantity, update.Data.Price, update.Data.TradeTime)
            {
                Side = update.Data.BuyerIsMaker ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            } })), ct).ConfigureAwait(false);

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
            var result = await SubscribeToKlineUpdatesAsync(symbol, interval, update => handler(update.AsExchangeEvent(Exchange, new SharedKline(update.Data.Kline.OpenTime, update.Data.Kline.ClosePrice, update.Data.Kline.HighPrice, update.Data.Kline.LowPrice, update.Data.Kline.OpenPrice, update.Data.Kline.Volume))), ct).ConfigureAwait(false);

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

            var result = await SubscribeToBalanceUpdatesAsync(request.ListenKey!,
                update => handler(update.AsExchangeEvent<SharedBalance[]>(Exchange, update.Data.EventData.Balances.Select(x => new SharedBalance(x.Asset, x.Total, x.Total + x.Locked)).ToArray())),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Spot Order client
        EndpointOptions<SubscribeSpotOrderRequest> ISpotOrderSocketClient.SubscribeSpotOrderOptions { get; } = new EndpointOptions<SubscribeSpotOrderRequest>(false)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SubscribeSpotOrderRequest.ListenKey), typeof(string), "The listenkey for starting the user stream", "123123123")
            }
        };
        async Task<ExchangeResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<ExchangeEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
        {
            var validationError = ((ISpotOrderSocketClient)this).SubscribeSpotOrderOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToOrderUpdatesAsync(request.ListenKey!,
                update => handler(update.AsExchangeEvent<SharedSpotOrder[]>(Exchange, new[] {
                    new SharedSpotOrder(
                        ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        ParseOrderType(update.Data.Type),
                        update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        update.Data.Status == Enums.OrderStatus.Canceled ? SharedOrderStatus.Canceled : (update.Data.Status == Enums.OrderStatus.New || update.Data.Status == Enums.OrderStatus.Pending || update.Data.Status == Enums.OrderStatus.PartiallyFilled) ? SharedOrderStatus.Open : SharedOrderStatus.Filled,
                        update.Data.CreateTime)
                    {
                        ClientOrderId = update.Data.ClientOrderId,
                        OrderPrice = update.Data.Price,
                        OrderQuantity = new SharedOrderQuantity(update.Data.Quantity, update.Data.QuoteOrderQuantity == 0 ? null : update.Data.QuoteOrderQuantity, null),
                        QuantityFilled = new SharedOrderQuantity(update.Data.QuantityFilled, update.Data.VolumeFilled, null),
                        Fee = update.Data.Fee.HasValue ? Math.Abs(update.Data.Fee.Value) : null,
                        FeeAsset = update.Data.FeeAsset,
                        UpdateTime = update.Data.UpdateTime,
                        IsTriggerOrder = update.Data.Type == OrderType.StopLimit || update.Data.Type == OrderType.StopMarket || update.Data.Type == OrderType.TriggerLimit || update.Data.Type == OrderType.TriggerMarket,
                        LastTrade = update.Data.LastFillQuantity > 0 ? new SharedUserTrade(ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol), update.Data.Symbol, update.Data.OrderId.ToString(), update.Data.TradeId.ToString(), update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell, update.Data.LastFillQuantity!.Value, update.Data.LastFillPrice!.Value, update.Data.UpdateTime!.Value) : null
                    }
                })),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        private SharedOrderType ParseOrderType(OrderType type)
        {
            if (type == OrderType.Market || type == OrderType.StopMarket)
                return SharedOrderType.Market;

            if (type == OrderType.Limit || type == OrderType.StopLimit)
                return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        #endregion

    }
}
