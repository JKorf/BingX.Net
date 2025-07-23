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

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BingXSubscription<T> : Subscription<BingXSocketResponse, BingXSocketResponse>
    {
        private readonly string _topic;
        private readonly Action<DataEvent<T>> _handler;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataType"></param>
        /// <param name="listenId"></param>
        /// <param name="handler"></param>
        /// <param name="auth"></param>
        public BingXSubscription(ILogger logger, string dataType, string listenId, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = dataType;

            MessageMatcher = MessageMatcher.Create<BingXUpdate<T>>(listenId, DoHandleMessage);
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
