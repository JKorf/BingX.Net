using BingX.Net.Objects.Internal;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXPingSubscription : SystemSubscription
    {
        public BingXPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<BingXPing>("ping", HandleMessage);
        }

        public CallResult HandleMessage(SocketConnection connection, DataEvent<BingXPing> message)
        {
            connection.Send(ExchangeHelpers.NextId(), new BingXPong { Pong = message.Data.Ping, Timestamp = message.Data.Timestamp }, 1);
            return CallResult.SuccessResult;
        }
    }
}
