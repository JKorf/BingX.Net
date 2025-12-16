using BingX.Net.Objects.Internal;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using System;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXPingSubscription : SystemSubscription
    {
        public BingXPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<BingXPing>("ping", HandleMessage);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<BingXPing>("ping", HandleMessage);
        }

        public CallResult HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXPing message)
        {
            _ = connection.SendAsync(ExchangeHelpers.NextId(), new BingXPong { Pong = message.Ping, Timestamp = message.Timestamp }, 1);
            return CallResult.SuccessResult;
        }
    }
}
