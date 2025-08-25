using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Interfaces;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Clients;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BingXSubscription<T> : Subscription<BingXSocketResponse, BingXSocketResponse>
    {
        private readonly SocketApiClient _client;
        private readonly string _topic;
        private readonly Action<DataEvent<T>> _handler;

        /// <summary>
        /// ctor
        /// </summary>
        public BingXSubscription(ILogger logger, SocketApiClient client, string dataType, string listenId, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _topic = dataType;

            MessageMatcher = MessageMatcher.Create<BingXUpdate<T>>(listenId, DoHandleMessage);
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
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<BingXUpdate<T>> message)
        {
            if (message.Data is BingXUpdate<BingXFuturesKlineUpdate[]> klineUpdates)
            {
                foreach (var klineUpdate in klineUpdates.Data!)
                    klineUpdate.Symbol = message.Data.Symbol!;
            }

            _handler.Invoke(message.As(message.Data.Data!, message.Data.DataType, message.Data.Symbol, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
