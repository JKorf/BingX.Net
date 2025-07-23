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
        private readonly string _topic;
        private readonly Action<DataEvent<BingXBalanceUpdate>> _handler;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="handler"></param>
        public BingXBalanceSubscription(ILogger logger, Action<DataEvent<BingXBalanceUpdate>> handler) : base(logger, false)
        {
            _handler = handler;
            _topic = "ACCOUNT_UPDATE";

            MessageMatcher = MessageMatcher.Create<BingXBalanceUpdate>(_topic, DoHandleMessage);
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
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<BingXBalanceUpdate> message)
        {
            _handler.Invoke(message.As(message.Data!, message.Data.Event, null, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
