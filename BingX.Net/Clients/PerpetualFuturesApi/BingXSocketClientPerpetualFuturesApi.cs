using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using BingX.Net.Objects.Options;
using BingX.Net.Enums;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Clients;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using CryptoExchange.Net.Objects.Sockets;
using System;
using BingX.Net.Objects.Models;
using System.Threading;
using CryptoExchange.Net.Objects;
using System.Threading.Tasks;
using BingX.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// Client providing access to the BingX futures websocket Api
    /// </summary>
    internal partial class BingXSocketClientPerpetualFuturesApi : SocketApiClient, IBingXSocketClientPerpetualFuturesApi
    {
        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        private static readonly MessagePath _dataTypePath = MessagePath.Get().Property("dataType");
        private static readonly MessagePath _eventPath = MessagePath.Get().Property("e");

        private static readonly MessagePath _acPath = MessagePath.Get().Property("ac");
        private static readonly MessagePath _aPath = MessagePath.Get().Property("a");

        protected override ErrorMapping ErrorMapping => BingXErrors.FuturesErrors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BingXSocketClientPerpetualFuturesApi(ILogger logger, BingXSocketOptions options) :
            base(logger, options.Environment.SocketClientSwapAddress!, options, options.FuturesOptions)
        {
            ProcessUnparsableMessages = true;

            AddSystemSubscription(new BingXFuturesPingSubscription(_logger));
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => BingXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext));
        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(SerializerOptions.WithConverters(BingXExchange._serializerContext));

        public IBingXSocketClientPerpetualFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTradeUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@trade";
            var subscription = new BingXSubscription<BingXFuturesTradeUpdate[]>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(x.Data.First().Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.TradeTime))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 5, 10, 20, 50, 100);
            updateInterval.ValidateIntValues(nameof(updateInterval),100, 200, 500, 1000);

            var stream = symbol + $"@depth{depth}@{updateInterval}ms";
            var subscription = new BingXSubscription<BingXOrderBook>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(symbol)
                .WithDataTimestamp(x.Data.Timestamp)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 5, 10, 20, 50, 100);
            updateInterval.ValidateIntValues(nameof(updateInterval), 100, 200, 500, 1000);

            var stream = $"all@depth{depth}@{updateInterval}ms";
            var subscription = new BingXSubscription<BingXOrderBook>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(x.Data.Symbol!)
                .WithDataTimestamp(x.Data.Timestamp)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var stream = "all@kline_" + EnumConverter.GetString(interval);
            var subscription = new BingXSubscription<BingXFuturesKlineUpdate[]>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@kline_" + EnumConverter.GetString(interval);
            var subscription = new BingXSubscription<BingXFuturesKlineUpdate[]>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@ticker";
            var subscription = new BingXSubscription<BingXFuturesTickerUpdate>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(x.Data.Symbol)
                .WithDataTimestamp(x.Data.EventTime)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = "all@ticker";
            var subscription = new BingXSubscription<BingXFuturesTickerUpdate>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(x.Data.Symbol)
                .WithDataTimestamp(x.Data.EventTime)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@lastPrice";
            var subscription = new BingXSubscription<BingXPriceUpdate>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(x.Data.Symbol)
                .WithDataTimestamp(x.Data.EventTime)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<BingXMarkPriceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@markPrice";
            var subscription = new BingXSubscription<BingXMarkPriceUpdate>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(x.Data.Symbol)
                .WithDataTimestamp(x.Data.EventTime)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@bookTicker";
            var subscription = new BingXSubscription<BingXBookTickerUpdate>(_logger, this, stream, stream, x => onMessage(
                x.WithStreamId(stream)
                .WithSymbol(x.Data.Symbol)
                .WithDataTimestamp(x.Data.EventTime)), false);
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

            var subscription = new BingXUserDataSubscription(_logger, onAccountUpdate, onOrderUpdate, onConfigurationUpdate, onListenKeyExpiredUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market") + "?listenKey=" + listenKey, subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override ReadOnlyMemory<byte> PreprocessStreamMessage(SocketConnection connection, WebSocketMessageType type, ReadOnlyMemory<byte> data)
        {
            if (type != WebSocketMessageType.Binary)
                return data;

            return data.DecompressGzip();
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            if (!message.IsValid && string.Equals(message.GetOriginalString(), "Ping", StringComparison.Ordinal))
                return "Ping";

            var id = message.GetValue<string>(_idPath);
            if (id != null)
                return id;

            var dataType = message.GetValue<string>(_dataTypePath);
            if (dataType != null)
                return dataType;

            var evnt = message.GetValue<string>(_eventPath);
            if (evnt!.Equals("SNAPSHOT", StringComparison.Ordinal))
            {
                if (message.GetNodeType(_acPath) != null)
                    return evnt + "AC";
                if (message.GetNodeType(_aPath) != null)
                    return evnt + "A";
            }

            return evnt;
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(null);
    }
}
