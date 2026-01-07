using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Sockets.Default;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BingXBalanceSubscription : Subscription
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
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<BingXBalanceUpdate>(_topic, DoHandleMessage);
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
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXBalanceUpdate message)
        {
            _client.UpdateTimeOffset(message.EventTime);

            _handler.Invoke(new DataEvent<BingXBalanceUpdate>(BingXExchange.ExchangeName, message, receiveTime, originalData)
                .WithUpdateType(SocketUpdateType.Update)
                .WithDataTimestamp(message.EventTime, _client.GetTimeOffset()));
            return CallResult.SuccessResult;
        }
    }
}
