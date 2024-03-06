using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BingX.Net.Interfaces.Clients.FuturesApi;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Options;
using BingX.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Clients;

namespace BingX.Net.Clients.FuturesApi
{
    /// <summary>
    /// Client providing access to the BingX futures websocket Api
    /// </summary>
    public class BingXSocketClientFuturesApi : SocketApiClient, IBingXSocketClientFuturesApi
    {
        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BingXSocketClientFuturesApi(ILogger logger, BingXSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.FuturesOptions)
        {
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        ///// <inheritdoc />
        //public async Task<CallResult<UpdateSubscription>> SubscribeToBingXUpdatesAsync(Action<DataEvent<object>> onMessage, CancellationToken ct = default)
        //{
        //    var subscription = new BingXSubscription<object>(_logger, "TOOD", "", onMessage, false);
        //    return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        //}

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            return message.GetValue<string>(_idPath);
        }

        /// <inheritdoc />
        protected override Query? GetAuthenticationRequest() => null;
    }
}
