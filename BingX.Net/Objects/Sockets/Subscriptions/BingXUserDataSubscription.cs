using BingX.Net.Objects.Models;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXUserDataSubscription : Subscription
    {
        private readonly Action<DataEvent<BingXFuturesOrderUpdate>>? _orderHandler;
        private readonly Action<DataEvent<BingXConfigUpdate>>? _configHandler;
        private readonly Action<DataEvent<BingXFuturesAccountUpdate>>? _accountHandler;
        private readonly Action<DataEvent<BingXListenKeyExpiredUpdate>>? _listenkeyHandler;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="orderHandler"></param>
        /// <param name="configHandler"></param>
        /// <param name="accountHandler"></param>
        /// <param name="listenkeyHandler"></param>
        public BingXUserDataSubscription(
            ILogger logger,
            Action<DataEvent<BingXFuturesAccountUpdate>>? accountHandler,
            Action<DataEvent<BingXFuturesOrderUpdate>>? orderHandler,
            Action<DataEvent<BingXConfigUpdate>>? configHandler,
            Action<DataEvent<BingXListenKeyExpiredUpdate>>? listenkeyHandler) : base(logger, false)
        {
            _orderHandler = orderHandler;
            _configHandler = configHandler;
            _accountHandler = accountHandler;
            _listenkeyHandler = listenkeyHandler;

            MessageMatcher = MessageMatcher.Create([
                new MessageHandlerLink<BingXListenKeyExpiredUpdate>("listenKeyExpired", DoHandleMessage),
                new MessageHandlerLink<BingXConfigUpdate>("ACCOUNT_CONFIG_UPDATE", DoHandleMessage),
                new MessageHandlerLink<BingXFuturesAccountUpdate>("ACCOUNT_UPDATE", DoHandleMessage),
                new MessageHandlerLink<BingXFuturesOrderUpdateWrapper>("ORDER_TRADE_UPDATE", DoHandleMessage),
                new MessageHandlerLink<BingXConfigUpdate>("SNAPSHOTAC", DoHandleMessage),
                new MessageHandlerLink<BingXFuturesAccountUpdate>("SNAPSHOTA", DoHandleMessage),
                ]);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<BingXConfigUpdate> message)
        {
            _configHandler?.Invoke(message.As(message.Data, message.Data.Event, message.Data.Configuration.Symbol, message.Data.Event == "SNAPSHOT" ? SocketUpdateType.Snapshot : SocketUpdateType.Update).WithDataTimestamp(message.Data.EventTime));
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<BingXFuturesAccountUpdate> message)
        {
            _accountHandler?.Invoke(message.As(message.Data, message.Data.Event, null, message.Data.Event == "SNAPSHOT" ? SocketUpdateType.Snapshot : SocketUpdateType.Update).WithDataTimestamp(message.Data.EventTime));
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<BingXFuturesOrderUpdateWrapper> message)
        {
            _orderHandler?.Invoke(message.As(message.Data.Data, message.Data.Event, message.Data.Data.Symbol, SocketUpdateType.Update).WithDataTimestamp(message.Data.EventTime));
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<BingXListenKeyExpiredUpdate> message)
        {
            _listenkeyHandler?.Invoke(message.As(message.Data!, message.Data.Event, null, SocketUpdateType.Update).WithDataTimestamp(message.Data.EventTime));
            return CallResult.SuccessResult;
        }
    }
}