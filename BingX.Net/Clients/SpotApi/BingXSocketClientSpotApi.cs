using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.MessageParsing;
using CryptoExchange.Net.Sockets.MessageParsing.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BingX.Net.Interfaces.Clients.FuturesApi;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Options;
using BingX.Net.Objects.Sockets.Subscriptions;
using System.IO.Compression;
using System.IO;
using System.Net.WebSockets;

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
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BingXSubscription<BingXTradeUpdate>(_logger, symbol + "@trade", symbol + "@trade", onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BingXSubscription<BingXTickerUpdate>(_logger, symbol + "@ticker", "24hTicker" + symbol, onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("market"), subscription, ct).ConfigureAwait(false);
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
    }
}
