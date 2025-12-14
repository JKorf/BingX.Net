using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using System;

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

            MessageRouter = MessageRouter.Create([
                MessageRoute<BingXListenKeyExpiredUpdate>.CreateWithoutTopicFilter("listenKeyExpired", DoHandleMessage),
                MessageRoute<BingXConfigUpdate>.CreateWithoutTopicFilter("ACCOUNT_CONFIG_UPDATE", DoHandleMessage),
                MessageRoute<BingXFuturesAccountUpdate>.CreateWithoutTopicFilter("ACCOUNT_UPDATE", DoHandleMessage),
                MessageRoute<BingXFuturesOrderUpdateWrapper>.CreateWithoutTopicFilter("ORDER_TRADE_UPDATE", DoHandleMessage),
                MessageRoute<BingXConfigUpdate>.CreateWithoutTopicFilter("SNAPSHOTAC", DoHandleMessage),
                MessageRoute<BingXFuturesAccountUpdate>.CreateWithoutTopicFilter("SNAPSHOTA", DoHandleMessage),
                ]);

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
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXConfigUpdate message)
        {
            _configHandler?.Invoke(
                new DataEvent<BingXConfigUpdate>(BingXExchange.ExchangeName, message, receiveTime, originalData)
                    .WithStreamId(message.Event)
                    .WithDataTimestamp(message.EventTime)
                    .WithSymbol(message.Configuration.Symbol)
                    .WithUpdateType(message.Event == "SNAPSHOT" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                );
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXFuturesAccountUpdate message)
        {
            _accountHandler?.Invoke(
                new DataEvent<BingXFuturesAccountUpdate>(BingXExchange.ExchangeName, message, receiveTime, originalData)
                    .WithStreamId(message.Event)
                    .WithDataTimestamp(message.EventTime)
                    .WithUpdateType(message.Event == "SNAPSHOT" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                );
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXFuturesOrderUpdateWrapper message)
        {
            _orderHandler?.Invoke(
                new DataEvent<BingXFuturesOrderUpdate>(BingXExchange.ExchangeName, message.Data, receiveTime, originalData)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithStreamId(message.Event)
                    .WithSymbol(message.Data.Symbol)
                    .WithDataTimestamp(message.EventTime)
                );

            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXListenKeyExpiredUpdate message)
        {
            _listenkeyHandler?.Invoke(
                new DataEvent<BingXListenKeyExpiredUpdate>(BingXExchange.ExchangeName, message, receiveTime, originalData)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithStreamId(message.Event)
                    .WithDataTimestamp(message.EventTime)
                );
            return CallResult.SuccessResult;
        }
    }
}