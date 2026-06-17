using BingX.Net.Clients.MessageHandlers;
using BingX.Net.Enums;
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
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{

    /// <summary>
    /// Client providing access to the BingX spot websocket Api
    /// </summary>
    internal partial class BingXSocketClientSpotApi : SocketApiClient<BingXEnvironment, BingXAuthenticationProvider, BingXCredentials>, IBingXSocketClientSpotApi
    {
        // No HighPerf websocket subscriptions because the data is received compressed and needs to be decompressed

        #region fields
        protected override ErrorMapping ErrorMapping => BingXErrors.SpotErrors;
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
        internal BingXSocketClientSpotApi(ILoggerFactory? loggerFactory, BingXSocketOptions options) :
            base(loggerFactory, BingXExchange.Metadata.Id, options.Environment.SocketClientSpotAddress!, options, options.FuturesOptions)
        {
            _loggerFactory = loggerFactory;

            AddSystemSubscription(new BingXPingSubscription(_logger));

            KeepAliveTimeout = TimeSpan.Zero;

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
        protected override BingXAuthenticationProvider CreateAuthenticationProvider(BingXCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => BingXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        public IBingXSocketClientSpotApiShared SharedClient => this;

        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new BingXSocketClientSpotApiMessageConverter();

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext));

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@trade";
            var subscription = new BingXSubscription<BingXTradeUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXKlineUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@kline_" + KlineIntervalToWebsocketString(interval);
            var subscription = new BingXSubscription<BingXKlineUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 5, 10, 20, 50, 100);

            var stream = symbol + "@depth" + depth;
            var subscription = new BingXSubscription<BingXOrderBook>(_logger, this, stream, x =>
            {
                if (x.Data.Timestamp != null)
                    UpdateTimeOffset(x.Data.Timestamp.Value);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(symbol)
                    .WithDataTimestamp(x.Data.Timestamp, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(string symbol, Action<DataEvent<BingXIncrementalOrderBook>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@incrDepth";
            var subscription = new BingXSubscription<BingXIncrementalOrderBook>(_logger, this, stream, x =>
            {
                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(symbol)
                    .WithSequenceNumber(x.Data.LastUpdateId));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@ticker";
            var subscription = new BingXSubscription<BingXTickerUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(
                    x.WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
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
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
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
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BingXOrderUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToOrderUpdatesAsync(null, onMessage, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string? listenKey, Action<DataEvent<BingXOrderUpdate>> onMessage, CancellationToken ct = default)
        {
            if (listenKey == null && !Authenticated)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, new NoApiCredentialsError());

            TokenLease? lease = null;
            if (listenKey == null)
            {
                var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    BingXExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Key), ct).ConfigureAwait(false);
                if (!leaseResult.Success)
                    return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

                lease = leaseResult.Data;
            }

            var stream = "spot.executionReport";
            var subscription = new BingXSubscription<BingXOrderUpdate>(_logger, this, stream, x =>
            {
                UpdateTimeOffset(x.Data.EventTime);

                onMessage(x
                    .WithStreamId(stream)
                    .WithSymbol(x.Data.Symbol)
                    .WithDataTimestamp(x.Data.EventTime, GetTimeOffset()));
            }, false)
            {
                TokenLease = lease
            };

            var lk = listenKey ?? lease!.Token.Token;
            var result = await SubscribeAsync(BaseAddress.AppendPath("market") + "?listenKey=" + lk, subscription, ct).ConfigureAwait(false);
            if (!result.Success && lease != null)
                await lease.ReleaseAsync().ConfigureAwait(false);

            return result;
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BingXBalanceUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToBalanceUpdatesAsync(null, onMessage, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(string? listenKey, Action<DataEvent<BingXBalanceUpdate>> onMessage, CancellationToken ct = default)
        {
            if (listenKey == null && !Authenticated)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, new NoApiCredentialsError());

            TokenLease? lease = null;
            if (listenKey == null)
            {
                var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    BingXExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Key), ct).ConfigureAwait(false);
                if (!leaseResult.Success)
                    return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

                lease = leaseResult.Data;
            }

            var lk = listenKey ?? lease!.Token.Token;
            var subscription = new BingXBalanceSubscription(_logger, this, onMessage)
            {
                TokenLease = lease
            };
            var result = await SubscribeAsync(BaseAddress.AppendPath("market") + "?listenKey=" + lk, subscription, ct).ConfigureAwait(false);
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

        private string KlineIntervalToWebsocketString(KlineInterval interval) => interval switch
        {
            KlineInterval.OneMinute => "1min",
            KlineInterval.ThreeMinutes => "3min",
            KlineInterval.FiveMinutes => "5min",
            KlineInterval.FifteenMinutes => "15min",
            KlineInterval.ThirtyMinutes => "30min",
            KlineInterval.OneHour => "60min",
            KlineInterval.TwoHours => "2hour",
            KlineInterval.FourHours => "4hour",
            KlineInterval.SixHours => "6hour",
            KlineInterval.EightHours => "8hour",
            KlineInterval.TwelveHours => "12hour",
            KlineInterval.OneDay => "1day",
            KlineInterval.OneWeek => "1week",
            KlineInterval.OneMonth => "1month",
            _ => throw new InvalidDataException("Unknown interval")
        };

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
                    "Spot",
                    ApiCredentials!.Key);

            var token = await TokenManager.AcquireAndReplaceAsync(subscriptions[0], scope).ConfigureAwait(false);
            if (!token.Success)
                return null;

            return new Uri(BaseAddress.AppendPath("market") + "?listenKey=" + token.Data.Token.Token);
        }

        private async Task<CallResult<string>> StartListenKeyAsync(TokenScope tokenScope, CancellationToken ct)
        {
            var result = await TokenClient.SpotApi.Account.StartUserStreamAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok(result.Data);
        }

        private async Task<CallResult> KeepAliveListenKeyAsync(TokenInfo token, CancellationToken ct)
        {
            var result = await TokenClient.SpotApi.Account.KeepAliveUserStreamAsync(token.Token, ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok();
        }

        private async Task<CallResult> StopListenKeyAsync(TokenInfo token, CancellationToken ct)
        {
            var result = await TokenClient.SpotApi.Account.StopUserStreamAsync(token.Token, ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok();
        }
    }
}
