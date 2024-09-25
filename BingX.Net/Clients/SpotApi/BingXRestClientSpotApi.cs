using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.MessageParsing;
using System.Linq;
using System.Globalization;
using BingX.Net.Enums;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBingXRestClientSpotApi" />
    internal partial class BingXRestClientSpotApi : RestApiClient, IBingXRestClientSpotApi, ISpotClient
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Spot Api");
        #endregion

        internal new BingXRestOptions ClientOptions => (BingXRestOptions)base.ClientOptions;

        #region Api clients
        /// <inheritdoc />
        public IBingXRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IBingXRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBingXRestClientSpotApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "BingX";
        #endregion

        /// <summary>
        /// Event triggered when an order is placed via this client. Only available for Spot orders
        /// </summary>
        public event Action<OrderId>? OnOrderPlaced;
        /// <summary>
        /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
        /// </summary>
        public event Action<OrderId>? OnOrderCanceled;

        #region constructor/destructor
        internal BingXRestClientSpotApi(ILogger logger, HttpClient? httpClient, BingXRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.SpotOptions)
        {
            Account = new BingXRestClientSpotApiAccount(this);
            ExchangeData = new BingXRestClientSpotApiExchangeData(logger, this);
            Trading = new BingXRestClientSpotApiTrading(logger, this);

            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InUri;
            RequestBodyFormat = RequestBodyFormat.FormData;
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null) => baseAsset.ToUpperInvariant() + "-" + quoteAsset.ToUpperInvariant();

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();
        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        internal Uri GetUri(string path) => new Uri(BaseAddress.AppendPath(path));

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            return await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null, Dictionary<string, string>? additionalHeaders = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight, additionalHeaders);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null, Dictionary<string, string>? additionalHeaders = null) where T : class
        {
            var result = await base.SendAsync<BingXResult<T>>(baseAddress, definition, parameters, cancellationToken, additionalHeaders, weight).ConfigureAwait(false);
            if (!result.Success)
                return result.As<T>(null);

            if (result.Data.Code != 0)
                return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message!));

            return result.As<T>(result.Data.Data);
        }

        internal Task<WebCallResult<T>> SendRawAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendRawToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendRawToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            return await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override ServerError? TryParseError(IMessageAccessor accessor)
        {
            var code = accessor.GetValue<int>(MessagePath.Get().Property("code"));
            if (code == 0)
                return null;

            var msg = accessor.GetValue<string>(MessagePath.Get().Property("msg"));
            return new ServerError(code, msg!);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp, ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

        /// <inheritdoc />
        public ISpotClient CommonSpotClient => this;
        public IBingXRestClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, IDictionary<string, object> parameters, string contentType)
        {
            var stringData = parameters.CreateParamString(false, ArraySerialization);
            request.SetContent(stringData, contentType);
        }

        /// <inheritdoc />
        public string GetSymbolName(string baseAsset, string quoteAsset) => baseAsset + "-" + quoteAsset;

        internal void InvokeOrderPlaced(OrderId id)
        {
            OnOrderPlaced?.Invoke(id);
        }

        internal void InvokeOrderCanceled(OrderId id)
        {
            OnOrderCanceled?.Invoke(id);
        }

        async Task<WebCallResult<OrderId>> ISpotClient.PlaceOrderAsync(string symbol, CommonOrderSide side, CommonOrderType type, decimal quantity, decimal? price, string? accountId, string? clientOrderId, CancellationToken ct)
        {
            var order = await Trading.PlaceOrderAsync(symbol,
                side == CommonOrderSide.Sell ? Enums.OrderSide.Sell : Enums.OrderSide.Buy,
                type == CommonOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity,
                price,
                clientOrderId: clientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!order)
                return order.As<OrderId>(null);

            return order.As(new OrderId
            {
                SourceObject = order,
                Id = order.Data.OrderId.ToString(CultureInfo.InvariantCulture)
            });
        }

        async Task<WebCallResult<Order>> IBaseRestClient.GetOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetOrderAsync), nameof(symbol));

            if (!long.TryParse(orderId, out var longId))
                throw new ArgumentException(nameof(symbol) + " invalid orderId for BingX " + nameof(ISpotClient.GetOrderAsync), nameof(symbol));

            var order = await Trading.GetOrderAsync(symbol!, longId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.As<Order>(null);

            return order.As(new Order
            {
                SourceObject = order.Data,
                Id = order.Data.OrderId.ToString(CultureInfo.InvariantCulture),
                Price = order.Data.Price,
                Quantity = order.Data.Quantity,
                QuantityFilled = order.Data.QuantityFilled,
                Side = order.Data.Side == Enums.OrderSide.Buy ? CommonOrderSide.Buy: CommonOrderSide.Sell,
                Symbol = order.Data.Symbol,
                Timestamp = order.Data.CreateTime,
                Type = order.Data.Type == Enums.OrderType.Limit ? CommonOrderType.Limit : order.Data.Type == Enums.OrderType.Market ? CommonOrderType.Market : CommonOrderType.Other,
                Status = order.Data.Status == Enums.OrderStatus.Canceled ? CommonOrderStatus.Canceled: order.Data.Status == Enums.OrderStatus.Filled ? CommonOrderStatus.Filled : order.Data.Status == Enums.OrderStatus.Failed ? CommonOrderStatus.Canceled : CommonOrderStatus.Active
            });
        }

        async Task<WebCallResult<IEnumerable<UserTrade>>> IBaseRestClient.GetOrderTradesAsync(string orderId, string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetOrderTradesAsync), nameof(symbol));

            if (!long.TryParse(orderId, out var longId))
                throw new ArgumentException(nameof(symbol) + " invalid orderId for BingX " + nameof(ISpotClient.GetOrderTradesAsync), nameof(symbol));

            var trades = await Trading.GetUserTradesAsync(symbol!, longId, ct: ct).ConfigureAwait(false);
            if (!trades)
                return trades.As<IEnumerable<UserTrade>>(null);

            return trades.As<IEnumerable<UserTrade>>(trades.Data.Select(t => new UserTrade
            {
                SourceObject = t,
                Fee = t.Fee,
                FeeAsset = t.FeeAsset,
                Id = t.Id.ToString(),
                OrderId = t.OrderId.ToString(),
                Price = t.Price,
                Quantity = t.Quantity,
                Symbol = t.Symbol,
                Timestamp = t.Timestamp
            }));
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetOpenOrdersAsync(string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetOpenOrdersAsync), nameof(symbol));

            var order = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.As<IEnumerable<Order>>(null);

            return order.As(order.Data.Select(order => new Order
            {
                SourceObject = order,
                Id = order.OrderId.ToString(CultureInfo.InvariantCulture),
                Price = order.Price,
                Quantity = order.Quantity,
                QuantityFilled = order.QuantityFilled,
                Side = order.Side == Enums.OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
                Symbol = order.Symbol,
                Timestamp = order.CreateTime,
                Type = order.Type == Enums.OrderType.Limit ? CommonOrderType.Limit : order.Type == Enums.OrderType.Market ? CommonOrderType.Market : CommonOrderType.Other,
                Status = order.Status == Enums.OrderStatus.Canceled ? CommonOrderStatus.Canceled : order.Status == Enums.OrderStatus.Filled ? CommonOrderStatus.Filled : order.Status == Enums.OrderStatus.Failed ? CommonOrderStatus.Canceled : CommonOrderStatus.Active
            }));
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetClosedOrdersAsync(string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetClosedOrdersAsync), nameof(symbol));

            var order = await Trading.GetOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.As<IEnumerable<Order>>(null);

            return order.As(order.Data.Select(order => new Order
            {
                SourceObject = order,
                Id = order.OrderId.ToString(CultureInfo.InvariantCulture),
                Price = order.Price,
                Quantity = order.Quantity,
                QuantityFilled = order.QuantityFilled,
                Side = order.Side == Enums.OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
                Symbol = order.Symbol,
                Timestamp = order.CreateTime,
                Type = order.Type == Enums.OrderType.Limit ? CommonOrderType.Limit : order.Type == Enums.OrderType.Market ? CommonOrderType.Market : CommonOrderType.Other,
                Status = order.Status == Enums.OrderStatus.Canceled ? CommonOrderStatus.Canceled : order.Status == Enums.OrderStatus.Filled ? CommonOrderStatus.Filled : order.Status == Enums.OrderStatus.Failed ? CommonOrderStatus.Canceled : CommonOrderStatus.Active
            }));
        }

        async Task<WebCallResult<OrderId>> IBaseRestClient.CancelOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.CancelOrderAsync), nameof(symbol));

            if (!long.TryParse(orderId, out var longId))
                throw new ArgumentException(nameof(symbol) + " invalid orderId for BingX " + nameof(ISpotClient.CancelOrderAsync), nameof(symbol));

            var order = await Trading.CancelOrderAsync(symbol!, longId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.As<OrderId>(null);

            return order.As(new OrderId
            {
                SourceObject = order,
                Id = order.Data.OrderId.ToString()
            });
        }

        async Task<WebCallResult<IEnumerable<Symbol>>> IBaseRestClient.GetSymbolsAsync(CancellationToken ct)
        {
            var symbols = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!symbols)
                return symbols.As<IEnumerable<Symbol>>(null);

            return symbols.As<IEnumerable<Symbol>>(symbols.Data.Select(t => new Symbol
            {
                SourceObject = t,
                MinTradeQuantity = t.MinOrderQuantity,
                Name = t.Name,
                QuantityStep = t.StepSize,
                PriceStep = t.TickSize
            }));
        }

        async Task<WebCallResult<Ticker>> IBaseRestClient.GetTickerAsync(string symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetTickerAsync), nameof(symbol));

            var ticker = await ExchangeData.GetTickersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!ticker)
                return ticker.As<Ticker>(null);

            return ticker.As(new Ticker
            {
                SourceObject = ticker,
                HighPrice = ticker.Data.Single().HighPrice,
                LastPrice = ticker.Data.Single().LastPrice,
                LowPrice = ticker.Data.Single().LowPrice,
                Symbol = ticker.Data.Single().Symbol,
                Volume = ticker.Data.Single().Volume,
                Price24H = ticker.Data.Single().OpenPrice
            });
        }

        async Task<WebCallResult<IEnumerable<Ticker>>> IBaseRestClient.GetTickersAsync(CancellationToken ct)
        {
            var symbols = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!symbols)
                return symbols.As<IEnumerable<Ticker>>(null);

            return symbols.As<IEnumerable<Ticker>>(symbols.Data.Select(t => new Ticker
            {
                SourceObject = t,
                HighPrice = t.HighPrice,
                LastPrice = t.LastPrice,
                LowPrice = t.LowPrice,
                Symbol = t.Symbol,
                Volume = t.Volume,
                Price24H = t.OpenPrice
            }));
        }

        async Task<WebCallResult<IEnumerable<Kline>>> IBaseRestClient.GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime, DateTime? endTime, int? limit, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetKlinesAsync), nameof(symbol));

            var symbols = await ExchangeData.GetKlinesAsync(symbol, GetKlineIntervalFromTimespan(timespan), startTime, endTime, ct: ct).ConfigureAwait(false);
            if (!symbols)
                return symbols.As<IEnumerable<Kline>>(null);

            return symbols.As<IEnumerable<Kline>>(symbols.Data.Select(t => new Kline
            {
                SourceObject = t,
                HighPrice = t.HighPrice,
                ClosePrice = t.ClosePrice,
                LowPrice = t.LowPrice,
                OpenPrice = t.OpenPrice,
                Volume = t.Volume,
                OpenTime = t.OpenTime
            }));
        }

        async Task<WebCallResult<OrderBook>> IBaseRestClient.GetOrderBookAsync(string symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetOrderBookAsync), nameof(symbol));

            var book = await ExchangeData.GetOrderBookAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!book)
                return book.As<OrderBook>(null);

            return book.As(new OrderBook
            {
                SourceObject = book,
                Asks = book.Data.Asks.Select(a => new OrderBookEntry { Price = a.Price, Quantity = a.Quantity }),
                Bids = book.Data.Bids.Select(b => new OrderBookEntry { Price = b.Price, Quantity = b.Quantity })
            });
        }

        async Task<WebCallResult<IEnumerable<Trade>>> IBaseRestClient.GetRecentTradesAsync(string symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol) + " required for BingX " + nameof(ISpotClient.GetRecentTradesAsync), nameof(symbol));

            var trades = await ExchangeData.GetRecentTradesAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!trades)
                return trades.As<IEnumerable<Trade>>(null);

            return trades.As<IEnumerable<Trade>>(trades.Data.Select(t => new Trade
            {
                SourceObject = t,
                Price = t.Price,
                Quantity = t.Quantity,
                Symbol = symbol,
                Timestamp = t.Timestamp
            }));
        }

        async Task<WebCallResult<IEnumerable<Balance>>> IBaseRestClient.GetBalancesAsync(string? accountId, CancellationToken ct)
        {
            var balances = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!balances)
                return balances.As<IEnumerable<Balance>>(null);

            return balances.As<IEnumerable<Balance>>(balances.Data.Select(t => new Balance
            {
                SourceObject = t,
                Asset = t.Asset,
                Available = t.Free,
                Total = t.Total
            }));
        }

        private static KlineInterval GetKlineIntervalFromTimespan(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.FromMinutes(1)) return KlineInterval.OneMinute;
            if (timeSpan == TimeSpan.FromMinutes(3)) return KlineInterval.ThreeMinutes;
            if (timeSpan == TimeSpan.FromMinutes(5)) return KlineInterval.FiveMinutes;
            if (timeSpan == TimeSpan.FromMinutes(15)) return KlineInterval.FifteenMinutes;
            if (timeSpan == TimeSpan.FromMinutes(30)) return KlineInterval.ThirtyMinutes;
            if (timeSpan == TimeSpan.FromHours(1)) return KlineInterval.OneHour;
            if (timeSpan == TimeSpan.FromHours(2)) return KlineInterval.TwoHours;
            if (timeSpan == TimeSpan.FromHours(4)) return KlineInterval.FourHours;
            if (timeSpan == TimeSpan.FromHours(6)) return KlineInterval.SixHours;
            if (timeSpan == TimeSpan.FromHours(8)) return KlineInterval.EightHours;
            if (timeSpan == TimeSpan.FromHours(12)) return KlineInterval.TwelveHours;
            if (timeSpan == TimeSpan.FromDays(1)) return KlineInterval.OneDay;
            if (timeSpan == TimeSpan.FromDays(3)) return KlineInterval.ThreeDay;
            if (timeSpan == TimeSpan.FromDays(7)) return KlineInterval.OneWeek;
            if (timeSpan == TimeSpan.FromDays(30) || timeSpan == TimeSpan.FromDays(31)) return KlineInterval.OneMonth;

            throw new ArgumentException("Unsupported timespan for BingX Klines, check supported intervals using BingX.Net.Enums.KlineInterval");
        }
    }
}
