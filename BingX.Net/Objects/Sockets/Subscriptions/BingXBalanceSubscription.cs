using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using CryptoExchange.Net.Interfaces;
using BingX.Net.Objects.Models;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BingXBalanceSubscription : Subscription<BingXSocketResponse, BingXSocketResponse>
    {
        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly string _topic;
        private readonly Action<DataEvent<BingXBalanceUpdate>> _handler;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(BingXBalanceUpdate);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="handler"></param>
        public BingXBalanceSubscription(ILogger logger, Action<DataEvent<BingXBalanceUpdate>> handler) : base(logger, false)
        {
            _handler = handler;
            _topic = "ACCOUNT_UPDATE";
            ListenerIdentifiers = new HashSet<string>() { _topic };
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection)
            => new BingXQuery(new BingXSocketRequest
            {
                Id = ExchangeHelpers.NextId().ToString(),
                RequestType = "sub",
                Topic = _topic
            }, false);

        /// <inheritdoc />
        public override Query? GetUnsubQuery()
            => new BingXQuery(new BingXSocketRequest
            {
                Id = ExchangeHelpers.NextId().ToString(),
                RequestType = "unsub",
                Topic = _topic
            }, false);

        /// <inheritdoc />
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var update = (BingXBalanceUpdate)message.Data;
            _handler.Invoke(message.As(update!, update.Event, SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
