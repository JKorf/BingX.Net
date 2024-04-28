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

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// Client providing access to the BingX futures websocket Api
    /// </summary>
    public class BingXSocketClientPerpetualFuturesApi : SocketApiClient, IBingXSocketClientPerpetualFuturesApi
    {
        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        private static readonly MessagePath _dataTypePath = MessagePath.Get().Property("dataType");
        private static readonly MessagePath _eventPath = MessagePath.Get().Property("e");
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
        public override string FormatSymbol(string baseAsset, string quoteAsset) => baseAsset.ToUpperInvariant() + "-" + quoteAsset.ToUpperInvariant();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();
        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor() => new SystemTextJsonByteMessageAccessor();

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<BingXFuturesTradeUpdate>>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BingXSubscription<IEnumerable<BingXFuturesTradeUpdate>>(_logger, symbol + "@trade", symbol + "@trade", onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 5, 10, 20, 50, 100);
            updateInterval.ValidateIntValues(nameof(updateInterval),100, 200, 500, 1000);

            var stream = symbol + $"@depth{depth}@{updateInterval}ms";
            var subscription = new BingXSubscription<BingXOrderBook>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<IEnumerable<BingXFuturesKlineUpdate>>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@kline_" + EnumConverter.GetString(interval);
            var subscription = new BingXSubscription<IEnumerable<BingXFuturesKlineUpdate>>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BingXSubscription<BingXFuturesTickerUpdate>(_logger, symbol + "@ticker", symbol + "@ticker", onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@lastPrice";
            var subscription = new BingXSubscription<BingXPriceUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<BingXMarkPriceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@markPrice";
            var subscription = new BingXSubscription<BingXMarkPriceUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@bookTicker";
            var subscription = new BingXSubscription<BingXBookTickerUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey, 
            Action<DataEvent<BingXFuturesAccountUpdate>> onAccountUpdate,
            Action<DataEvent<BingXFuturesOrderUpdate>> onOrderUpdate,
            Action<DataEvent<BingXConfigUpdate>> onConfigurationUpdate,
            Action<DataEvent<BingXListenKeyExpiredUpdate>> onListenKeyExpiredUpdate,
            CancellationToken ct = default)
        {
            var subscription = new BingXUserDataSubscription(_logger, onAccountUpdate, onOrderUpdate, onConfigurationUpdate, onListenKeyExpiredUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("swap-market") + "?listenKey=" + listenKey, subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override ReadOnlyMemory<byte> PreprocessStreamMessage(WebSocketMessageType type, ReadOnlyMemory<byte> data)
        {
            if (type != WebSocketMessageType.Binary)
                return data;

            return data.DecompressGzip();
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            if (!message.IsJson && string.Equals(message.GetOriginalString(), "Ping", StringComparison.Ordinal))
                return "Ping";

            var id = message.GetValue<string>(_idPath);
            if (id != null)
                return id;

            var dataType = message.GetValue<string>(_dataTypePath);
            if (dataType != null)
                return dataType;

            return message.GetValue<string>(_eventPath);
        }

        /// <inheritdoc />
        protected override Query? GetAuthenticationRequest() => null;
    }
}
