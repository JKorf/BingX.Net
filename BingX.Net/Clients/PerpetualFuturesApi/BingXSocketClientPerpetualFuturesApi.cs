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
using Microsoft.Extensions.Logging;
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
    internal partial class BingXSocketClientPerpetualFuturesApi : SocketApiClient, IBingXSocketClientPerpetualFuturesApi
    {
        // No HighPerf websocket subscriptions because the data is received compressed and needs to be decompressed

        #region fields

        protected override ErrorMapping ErrorMapping => BingXErrors.FuturesErrors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BingXSocketClientPerpetualFuturesApi(ILogger logger, BingXSocketOptions options) :
            base(logger, options.Environment.SocketClientSwapAddress!, options, options.FuturesOptions)
        {
            AddSystemSubscription(new BingXFuturesPingSubscription(_logger));
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => BingXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);


        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new BingXSocketPerpetualFuturesMessageHandler();

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext));

        public IBingXSocketClientPerpetualFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTradeUpdate[]>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesIncrementalOrderBook>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var stream = "all@kline_" + EnumConverter.GetString(interval);
            var subscription = new BingXSubscription<BingXFuturesKlineUpdate[]>(_logger, this, stream, x => onMessage(
                x.WithStreamId(stream)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<BingXMarkPriceUpdate>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey, 
            Action<DataEvent<BingXFuturesAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<BingXFuturesOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<BingXConfigUpdate>>? onConfigurationUpdate = null,
            Action<DataEvent<BingXListenKeyExpiredUpdate>>? onListenKeyExpiredUpdate = null,
            CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var subscription = new BingXUserDataSubscription(_logger, this, onAccountUpdate, onOrderUpdate, onConfigurationUpdate, onListenKeyExpiredUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market") + "?listenKey=" + listenKey, subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<byte> PreprocessStreamMessage(SocketConnection connection, WebSocketMessageType type, ReadOnlySpan<byte> data)
        {
            if (type != WebSocketMessageType.Binary)
                return data;

            return data.DecompressGzip();
        }

    }
}
