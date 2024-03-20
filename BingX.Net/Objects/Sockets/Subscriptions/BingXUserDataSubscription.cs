using BingX.Net.Objects.Internal;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXUserDataSubscription : Subscription
    {
        private static readonly MessagePath _ePath = MessagePath.Get().Property("e");

        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        //private readonly Action<DataEvent<BinanceFuturesStreamOrderUpdate>>? _orderHandler;
        private readonly Action<DataEvent<BingXConfigUpdate>>? _configHandler;
        private readonly Action<DataEvent<BingXFuturesAccountUpdate>>? _accountHandler;
        private readonly Action<DataEvent<BingXListenKeyExpiredUpdate>>? _listenkeyHandler;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            var identifier = message.GetValue<string>(_ePath);
            if (identifier == "ACCOUNT_CONFIG_UPDATE")
                return typeof(BingXConfigUpdate);
            if (identifier == "ACCOUNT_UPDATE")
                return typeof(BingXFuturesAccountUpdate);
            //if (identifier == "ORDER_TRADE_UPDATE")
            //    return typeof(BinanceFuturesStreamOrderUpdate);
            if (identifier == "listenKeyExpired")
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
            //Action<DataEvent<BinanceFuturesStreamOrderUpdate>>? orderHandler,
            Action<DataEvent<BingXConfigUpdate>>? configHandler,
            Action<DataEvent<BingXListenKeyExpiredUpdate>>? listenkeyHandler) : base(logger, false)
        {
            //_orderHandler = orderHandler;
            _configHandler = configHandler;
            //_accountHandler = accountHandler;
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
            //else if (message.Data is BinanceFuturesStreamOrderUpdate orderUpdate)
            //{
            //    orderUpdate.Data.ListenKey = orderUpdate.Stream;
            //    _orderHandler?.Invoke(message.As(orderUpdate.Data, orderUpdate.Stream, SocketUpdateType.Update));
            //}
            //else
            if (message.Data is BingXListenKeyExpiredUpdate listenKeyUpdate)
            {
                _listenkeyHandler?.Invoke(message.As(listenKeyUpdate!, listenKeyUpdate!.ListenKey, SocketUpdateType.Update));
            }

            return new CallResult(null);
        }
    }
}