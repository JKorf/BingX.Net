using BingX.Net.Clients.SpotApi;
using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Options;
using BingX.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Interfaces;
using CryptoExchange.Net.TokenManagement;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// Client providing access to the BingX futures websocket Api
    /// </summary>
    internal partial class BingXSocketClientPerpetualFuturesApi : SocketApiClient<BingXEnvironment, BingXAuthenticationProvider, BingXCredentials>, IBingXSocketClientPerpetualFuturesApi
    {
        // No HighPerf websocket subscriptions because the data is received compressed and needs to be decompressed

        #region fields

        protected override ErrorMapping ErrorMapping => BingXErrors.FuturesErrors;
        private readonly ILoggerFactory? _loggerFactory;
        private BingXRestClient? _tokenClient;
        internal TokenManager TokenManager { get; }
        private BingXRestClient TokenClient
        {
            get
            {
                if (_tokenClient == null)
                {
                    _tokenClient = new BingXRestClient(null, _loggerFactory, Options.Create(new BingXRestOptions
                    {
                        ApiCredentials = ApiCredentials,
                        Environment = ClientOptions.Environment,
                        Proxy = ClientOptions.Proxy,
                        OutputOriginalData = ClientOptions.OutputOriginalData
                    }));
                }

                return _tokenClient;
            }
        }
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BingXSocketClientPerpetualFuturesApi(ILoggerFactory? loggerFactory, BingXSocketOptions options) :
            base(loggerFactory, BingXExchange.Metadata.Id, options.Environment.SocketClientSwapAddress!, options, options.FuturesOptions)
        {
            _loggerFactory = loggerFactory;

            AddSystemSubscription(new BingXFuturesPingSubscription(_logger));

            TokenManager = new TokenManager(
                BingXExchange.Metadata.Id,
                loggerFactory,
                TimeSpan.FromMinutes(30),
                TimeSpan.FromMinutes(60),
                startToken: StartListenKeyAsync,
                keepAliveToken: KeepAliveListenKeyAsync,
                stopToken: StopListenKeyAsync);
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => BingXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override BingXAuthenticationProvider CreateAuthenticationProvider(BingXCredentials credentials)
            => new BingXAuthenticationProvider(credentials);


        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new BingXSocketPerpetualFuturesMessageHandler();

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext));

        public IBingXSocketClientPerpetualFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTradeUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@trade";
            var subscription = new BingXSubscription<BingXFuturesTradeUpdate[]>(_logger, this, stream, x =>
            {
                var timestamp = x.Data.Max(x => x.TradeTime);
                UpdateTimeOffset(timestamp);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.First().Symbol)
                    .WithDataTimestamp(timestamp, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 5, 10, 20, 50, 100);
            updateInterval.ValidateIntValues(nameof(updateInterval),100, 200, 500, 1000);

            var stream = symbol + $"@depth{depth}@{updateInterval}ms";
            var subscription = new BingXSubscription<BingXOrderBook>(_logger, this, stream, x =>
            {
                if (x.Data.Timestamp != null)
                    UpdateTimeOffset(x.Data.Timestamp.Value);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(symbol)
                    .WithDataTimestamp(x.Data.Timestamp, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesIncrementalOrderBook>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@incrDepth";
            var subscription = new BingXSubscription<BingXFuturesIncrementalOrderBook>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.Timestamp);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(symbol)
                    .WithDataTimestamp(x.Data.Timestamp, GetTimeOffset())
                    .WithSequenceNumber(x.Data.LastUpdateId));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 5, 10, 20, 50, 100);
            updateInterval.ValidateIntValues(nameof(updateInterval), 100, 200, 500, 1000);

            var stream = $"all@depth{depth}@{updateInterval}ms";
            var subscription = new BingXSubscription<BingXOrderBook>(_logger, this, stream, x =>
            {
                if (x.Data.Timestamp != null)
                    UpdateTimeOffset(x.Data.Timestamp.Value);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol!)
                    .WithDataTimestamp(x.Data.Timestamp, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var stream = "all@kline_" + EnumConverter.GetString(interval);
            var subscription = new BingXSubscription<BingXFuturesKlineUpdate[]>(_logger, this, stream, x => onMessage(
                x.WithStreamId(stream)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@kline_" + EnumConverter.GetString(interval);
            var subscription = new BingXSubscription<BingXFuturesKlineUpdate[]>(_logger, this, stream, x =>
            {
                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(symbol));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@ticker";
            var subscription = new BingXSubscription<BingXFuturesTickerUpdate>(_logger, this, stream, x =>
            {
                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = "all@ticker";
            var subscription = new BingXSubscription<BingXFuturesTickerUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@lastPrice";
            var subscription = new BingXSubscription<BingXPriceUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<BingXMarkPriceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@markPrice";
            var subscription = new BingXSubscription<BingXMarkPriceUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@bookTicker";
            var subscription = new BingXSubscription<BingXBookTickerUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            Action<DataEvent<BingXFuturesAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<BingXFuturesOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<BingXConfigUpdate>>? onConfigurationUpdate = null,
            Action<DataEvent<BingXListenKeyExpiredUpdate>>? onListenKeyExpiredUpdate = null,
            CancellationToken ct = default)
            => SubscribeToUserDataUpdatesAsync(null, onAccountUpdate, onOrderUpdate, onConfigurationUpdate, onListenKeyExpiredUpdate, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string? listenKey, 
            Action<DataEvent<BingXFuturesAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<BingXFuturesOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<BingXConfigUpdate>>? onConfigurationUpdate = null,
            Action<DataEvent<BingXListenKeyExpiredUpdate>>? onListenKeyExpiredUpdate = null,
            CancellationToken ct = default)
        {
            if (listenKey == null && !Authenticated)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, new NoApiCredentialsError());

            TokenLease? lease = null;
            if (listenKey == null)
            {
                var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    BingXExchange.Metadata.Id,
                    EnvironmentName,
                    "Futures",
                    ApiCredentials!.Key), ct).ConfigureAwait(false);
                if (!leaseResult.Success)
                    return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

                lease = leaseResult.Data;
            }

            var subscription = new BingXUserDataSubscription(_logger, this, onAccountUpdate, onOrderUpdate, onConfigurationUpdate, onListenKeyExpiredUpdate)
            {
                TokenLease = lease
            };
            var lk = listenKey ?? lease!.Token.Token;
            var result = await SubscribeAsync(BaseAddress.AppendPath("swap-market") + "?listenKey=" + lk, subscription, ct).ConfigureAwait(false);
            if (!result.Success && lease != null)
                await lease.ReleaseAsync().ConfigureAwait(false);

            return result;
        }

        /// <inheritdoc />
        public override ReadOnlySpan<byte> PreprocessStreamMessage(SocketConnection connection, WebSocketMessageType type, ReadOnlySpan<byte> data)
        {
            if (type != WebSocketMessageType.Binary)
                return data;

            return data.DecompressGzip();
        }

        protected override async Task<Uri?> GetReconnectUriAsync(ISocketConnection connection)
        {
            if (!connection.HasAuthenticatedSubscription)
                return await base.GetReconnectUriAsync(connection).ConfigureAwait(false);

            var subscriptions = ((SocketConnection)connection).Subscriptions.Where(x => x.TokenLease != null).ToList();
            if (subscriptions.Count == 0)
                return await base.GetReconnectUriAsync(connection).ConfigureAwait(false);

            var scope = new TokenScope(
                    BingXExchange.Metadata.Id,
                    EnvironmentName,
                    "Futures",
                    ApiCredentials!.Key);

            var token = await TokenManager.AcquireAndReplaceAsync(subscriptions[0], scope).ConfigureAwait(false);
            if (!token.Success)
                return null;

            return new Uri(BaseAddress.AppendPath("swap-market") + "?listenKey=" + token.Data.Token.Token);
        }

        private async Task<CallResult<string>> StartListenKeyAsync(TokenScope tokenScope, CancellationToken ct)
        {
            var result = await TokenClient.PerpetualFuturesApi.Account.StartUserStreamAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok(result.Data);
        }

        private async Task<CallResult> KeepAliveListenKeyAsync(TokenInfo token, CancellationToken ct)
        {
            var result = await TokenClient.PerpetualFuturesApi.Account.KeepAliveUserStreamAsync(token.Token, ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok();
        }

        private async Task<CallResult> StopListenKeyAsync(TokenInfo token, CancellationToken ct)
        {
            var result = await TokenClient.PerpetualFuturesApi.Account.StopUserStreamAsync(token.Token, ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok();
        }
    }
}
