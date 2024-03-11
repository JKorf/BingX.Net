using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Options;
using BingX.Net.Objects.Sockets.Subscriptions;
using System.IO.Compression;
using System.IO;
using System.Net.WebSockets;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;

namespace BingX.Net.Clients.SpotApi
{
    /// <summary>
    /// Client providing access to the BingX spot websocket Api
    /// </summary>
    public class BingXSocketClientSpotApi : SocketApiClient, IBingXSocketClientSpotApi
    {
        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        private static readonly MessagePath _dataTypePath = MessagePath.Get().Property("dataType");
        private static readonly MessagePath _pingPath = MessagePath.Get().Property("ping");
        private static readonly MessagePath _eventPath = MessagePath.Get().Property("data").Property("e");
        private static readonly MessagePath _symbolPath = MessagePath.Get().Property("data").Property("s");
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BingXSocketClientSpotApi(ILogger logger, BingXSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.FuturesOptions)
        {
            AddSystemSubscription(new BingXPingSubscription(_logger));
        }
        #endregion 

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();
        /// <inheritdoc />
        protected override IMessageAccessor CreateAccessor() => new SystemTextJsonMessageAccessor();

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BingXSubscription<BingXTradeUpdate>(_logger, symbol + "@trade", symbol + "@trade", onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXKlineUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@kline_" + KlineIntervalToWebsocketString(interval);
            var subscription = new BingXSubscription<BingXKlineUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 5, 10, 20, 50, 100);

            var stream = symbol + "@depth" + depth;
            var subscription = new BingXSubscription<BingXOrderBook>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BingXSubscription<BingXTickerUpdate>(_logger, symbol + "@ticker", "24hTicker" + symbol, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@lastPrice";
            var subscription = new BingXSubscription<BingXPriceUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = symbol + "@bookTicker";
            var subscription = new BingXSubscription<BingXBookTickerUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string listenKey, Action<DataEvent<BingXOrderUpdate>> onMessage, CancellationToken ct = default)
        {
            // TODO Doesn't work?
            var stream = "spot.executionReport";
            var subscription = new BingXSubscription<BingXOrderUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market") + "?listenKey=" + listenKey, subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(string listenKey, Action<DataEvent<BingXBalanceUpdate>> onMessage, CancellationToken ct = default)
        {
            var stream = "ACCOUNT_UPDATE";
            var subscription = new BingXSubscription<BingXBalanceUpdate>(_logger, stream, stream, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market") + "?listenKey=" + listenKey, subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override Stream PreprocessStreamMessage(WebSocketMessageType type, Stream stream)
        {
            if (type != WebSocketMessageType.Binary)
                return stream;

            var decompressedStream = new MemoryStream();
            using var deflateStream = new GZipStream(stream, CompressionMode.Decompress);
            deflateStream.CopyTo(decompressedStream);
            decompressedStream.Position = 0;
            return decompressedStream;
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var id = message.GetValue<string>(_idPath);
            if (id != null)
                return id;

            var dataType = message.GetValue<string>(_dataTypePath);
            if (dataType != null)
                return dataType;

            var evnt = message.GetValue<string>(_eventPath);
            var symbol = message.GetValue<string>(_symbolPath);
            if (evnt != null)
                return evnt + symbol;

            var ping = message.GetValue<string>(_pingPath);
            return ping != null ? "ping" : ping;
        }

        /// <inheritdoc />
        protected override Query? GetAuthenticationRequest() => null;

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
    }
}
