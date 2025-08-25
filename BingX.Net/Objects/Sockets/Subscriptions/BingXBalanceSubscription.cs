using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using CryptoExchange.Net.Interfaces;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Clients;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BingXBalanceSubscription : Subscription<BingXSocketResponse, BingXSocketResponse>
    {
        private readonly string _topic;
        private readonly Action<DataEvent<BingXBalanceUpdate>> _handler;
        private readonly SocketApiClient _client;

        /// <summary>
        /// ctor
        /// </summary>
        public BingXBalanceSubscription(ILogger logger, SocketApiClient client, Action<DataEvent<BingXBalanceUpdate>> handler) : base(logger, false)
        {
            _client = client;
            _handler = handler;
            _topic = "ACCOUNT_UPDATE";

            MessageMatcher = MessageMatcher.Create<BingXBalanceUpdate>(_topic, DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
            => new BingXQuery(_client, new BingXSocketRequest
            {
                Id = ExchangeHelpers.NextId().ToString(),
                RequestType = "sub",
                Topic = _topic
            }, false);

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
            => new BingXQuery(_client, new BingXSocketRequest
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
