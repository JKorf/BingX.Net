using BingX.Net.Objects.Internal;
using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXFuturesPingSubscription : SystemSubscription
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string> { "Ping" };
        public override Type GetMessageType(IMessageAccessor message) => typeof(string);

        public BingXFuturesPingSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            connection.Send(ExchangeHelpers.NextId(), "Pong", 1);
            return new CallResult(null);
        }
    }
}
