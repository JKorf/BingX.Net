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

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBingXRestClientSpotApi" />
    public class BingXRestClientSpotApi : RestApiClient, IBingXRestClientSpotApi, ISpotClient
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Spot Api");
        #endregion

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
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();
        /// <inheritdoc />
        protected override IMessageAccessor CreateAccessor() => new SystemTextJsonMessageAccessor();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        internal Uri GetUri(string path) => new Uri(BaseAddress.AppendPath(path));

        internal async Task<WebCallResult<T>> SendRequestInternal<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        {
            var result = await SendRequestAsync<BingXResult<T>>(uri, method, cancellationToken, parameters, signed, null, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
            if (!result.Success)
                return result.As<T>(null);

            if (result.Data.Code != 0)
                return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message!));

            return result.As<T>(result.Data.Data);
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

        /// <inheritdoc />
        public string GetSymbolName(string baseAsset, string quoteAsset) =>
            throw new NotImplementedException();

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
            throw new NotImplementedException();
        }

        async Task<WebCallResult<Order>> IBaseRestClient.GetOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<UserTrade>>> IBaseRestClient.GetOrderTradesAsync(string orderId, string? symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetOpenOrdersAsync(string? symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetClosedOrdersAsync(string? symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<OrderId>> IBaseRestClient.CancelOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Symbol>>> IBaseRestClient.GetSymbolsAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<Ticker>> IBaseRestClient.GetTickerAsync(string symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Ticker>>> IBaseRestClient.GetTickersAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Kline>>> IBaseRestClient.GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime, DateTime? endTime, int? limit, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<OrderBook>> IBaseRestClient.GetOrderBookAsync(string symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Trade>>> IBaseRestClient.GetRecentTradesAsync(string symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Balance>>> IBaseRestClient.GetBalancesAsync(string? accountId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
