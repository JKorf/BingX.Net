using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using BingX.Net.Objects.Internal;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Sockets.Default;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BingXSubscription<T> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly string _topic;
        private readonly Action<DataEvent<T>> _handler;

        /// <summary>
        /// ctor
        /// </summary>
        public BingXSubscription(ILogger logger, SocketApiClient client, string listenId, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _topic = listenId;

            MessageRouter = MessageRouter.CreateWithoutTopicFilter<BingXUpdate<T>>(listenId, DoHandleMessage);
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
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXUpdate<T> message)
        {
            if (message.Timestamp != null)
                _client.UpdateTimeOffset(message.Timestamp.Value);

            if (message is BingXUpdate<BingXFuturesKlineUpdate[]> klineUpdates)
            {
                foreach (var klineUpdate in klineUpdates.Data!)
                    klineUpdate.Symbol = message.Symbol!;
            }

            _handler.Invoke(
                new DataEvent<T>(BingXExchange.ExchangeName, message.Data!, receiveTime, originalData)
                .WithUpdateType(SocketUpdateType.Update)
                .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.SuccessResult;
        }
    }
}
