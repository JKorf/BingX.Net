using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Interfaces;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BingXSubscription<T> : Subscription<BingXSocketResponse, BingXSocketResponse>
    {
        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly string _topic;
        private readonly Action<DataEvent<T>> _handler;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(BingXUpdate<T>);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataType"></param>
        /// <param name="listenId"></param>
        /// <param name="handler"></param>
        /// <param name="auth"></param>
        public BingXSubscription(ILogger logger, string dataType, string listenId, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = dataType;
            ListenerIdentifiers = new HashSet<string>() { listenId };
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
            var update = (BingXUpdate<T>)message.Data;
            _handler.Invoke(message.As(update.Data!, update.DataType, SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
