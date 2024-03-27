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
        private static readonly MessagePath _ePath = MessagePath.Get().Property("e");

        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly Action<DataEvent<BingXFuturesOrderUpdate>>? _orderHandler;
        private readonly Action<DataEvent<BingXConfigUpdate>>? _configHandler;
        private readonly Action<DataEvent<BingXFuturesAccountUpdate>>? _accountHandler;
        private readonly Action<DataEvent<BingXListenKeyExpiredUpdate>>? _listenkeyHandler;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            var identifier = message.GetValue<string>(_ePath);
            if (string.Equals(identifier, "ACCOUNT_CONFIG_UPDATE", StringComparison.Ordinal))
                return typeof(BingXConfigUpdate);
            if (string.Equals(identifier, "ACCOUNT_UPDATE", StringComparison.Ordinal))
                return typeof(BingXFuturesAccountUpdate);
            if (string.Equals(identifier, "ORDER_TRADE_UPDATE", StringComparison.Ordinal))
                return typeof(BingXFuturesOrderUpdateWrapper);
            if (string.Equals(identifier, "listenKeyExpired", StringComparison.Ordinal))
                return typeof(BingXListenKeyExpiredUpdate);

            return null;
        }

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
            HandleUpdatesBeforeConfirmation = true;
            ListenerIdentifiers = new HashSet<string>
            {
                "listenKeyExpired",
                "ACCOUNT_CONFIG_UPDATE",
                "ACCOUNT_UPDATE",
                "ORDER_TRADE_UPDATE"
            };
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        public override Query? GetUnsubQuery() => null;

        /// <inheritdoc />
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is BingXConfigUpdate configUpdate)
            {
                _configHandler?.Invoke(message.As(configUpdate, configUpdate.Configuration.Symbol, SocketUpdateType.Update));
            }
            else if (message.Data is BingXFuturesAccountUpdate accountUpdate)
            {
                _accountHandler?.Invoke(message.As(accountUpdate, accountUpdate.Update.Trigger, SocketUpdateType.Update));
            }
            else if (message.Data is BingXFuturesOrderUpdateWrapper orderUpdate)
            {
                _orderHandler?.Invoke(message.As(orderUpdate.Data, orderUpdate.Data.Symbol, SocketUpdateType.Update));
            }
            else if (message.Data is BingXListenKeyExpiredUpdate listenKeyUpdate)
            {
                _listenkeyHandler?.Invoke(message.As(listenKeyUpdate!, listenKeyUpdate!.ListenKey, SocketUpdateType.Update));
            }

            return new CallResult(null);
        }
    }
}