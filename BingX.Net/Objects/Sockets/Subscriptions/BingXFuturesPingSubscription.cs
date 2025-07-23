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
        public BingXFuturesPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<string>("Ping", DoHandleMessage);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<string> message)
        {
            connection.Send(ExchangeHelpers.NextId(), "Pong", 1);
            return CallResult.SuccessResult;
        }
    }
}
