using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using System;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXFuturesPingSubscription : SystemSubscription
    {
        public BingXFuturesPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<string>("Ping", DoHandleMessage);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, string message)
        {
            _ = connection.SendAsync(ExchangeHelpers.NextId(), "Pong", 1);
            return CallResult.SuccessResult;
        }
    }
}
