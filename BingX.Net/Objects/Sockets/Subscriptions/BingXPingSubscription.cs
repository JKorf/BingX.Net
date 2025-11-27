using BingX.Net.Objects.Internal;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXPingSubscription : SystemSubscription
    {
        public BingXPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<BingXPing>("ping", HandleMessage);
            MessageRouter = MessageRouter.Create<BingXPing>("ping", HandleMessage);
        }

        public CallResult HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXPing message)
        {
            _ = connection.SendAsync(ExchangeHelpers.NextId(), new BingXPong { Pong = message.Ping, Timestamp = message.Timestamp }, 1);
            return CallResult.SuccessResult;
        }
    }
}
