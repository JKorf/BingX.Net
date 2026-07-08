using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using BingX.Net.Enums;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX spot streams
    /// </summary>
    public interface IBingXSocketClientSpotApi : ISocketApiClient<BingXCredentials>, IDisposable
    {
        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        IBingXSocketClientSpotApiShared SharedClient { get; }

        /// <summary>
        /// Subscribe to live trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Market%20Data/Subscription%20transaction%20by%20transaction" /><br />
        /// Endpoint:<br />
        /// WS /market
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live kline/candlestick data updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Market%20Data/K-line%20Streamst" /><br />
        /// Endpoint:<br />
        /// WS /market
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">The interval to subscribe. Currently only the 1 minute interval is supported</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live orderbook data updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Market%20Data/Subscribe%20Market%20Depth%20Data" /><br />
        /// Endpoint:<br />
        /// WS /market
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="depth">The depth of the orderbook, 5, 10, 20, 50 or 100</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live incremental order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Market%20Data/Incremental%20and%20Full%20Depth%20Information" /><br />
        /// Endpoint:<br />
        /// WS /market
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(string symbol, Action<DataEvent<BingXIncrementalOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live price statistics updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Market%20Data/Subscribe%20to%2024-hour%20Price%20Change" /><br />
        /// Endpoint:<br />
        /// WS /market
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Market%20Data/Subscription%20transaction%20by%20transaction" /><br />
        /// Endpoint:<br />
        /// WS /market
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live best book price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Market%20Data/Subscription%20transaction%20by%20transaction" /><br />
        /// Endpoint:<br />
        /// WS /market
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates. Listen key is automatically obtained by the client and will be renewed as needed
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Account%20Data/order%20update%20event" /><br />
        /// Endpoint:<br />
        /// WS /market?listenKey={listenKey}
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BingXOrderUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates. Prior to using this, the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Account%20Data/order%20update%20event" /><br />
        /// Endpoint:<br />
        /// WS /market?listenKey={listenKey}
        /// </para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string listenKey, Action<DataEvent<BingXOrderUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user balance updates. Listen key is automatically obtained by the client and will be renewed as needed
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Account%20Data/Subscription%20account%20balance%20push" /><br />
        /// Endpoint:<br />
        /// WS /market?listenKey={listenKey}
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BingXBalanceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user balance updates. Prior to using this, the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Spot/Websocket%20Account%20Data/Subscription%20account%20balance%20push" /><br />
        /// Endpoint:<br />
        /// WS /market?listenKey={listenKey}
        /// </para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(string listenKey, Action<DataEvent<BingXBalanceUpdate>> onMessage, CancellationToken ct = default);
    }
}
