using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using BingX.Net.Enums;
using BingX.Net.Objects.Models;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX spot streams
    /// </summary>
    public interface IBingXSocketClientSpotApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        IBingXSocketClientSpotApiShared SharedClient { get; }

        /// <summary>
        /// Subscribe to live trade updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/market.html#Subscription%20transaction%20by%20transaction" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live kline/candlestick data updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/market.html#K-line%20Streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">The interval to subscribe. Currently only the 1 minute interval is supported</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live orderbook data updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/market.html#Subscribe%20Market%20Depth%20Data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="depth">The depth of the orderbook, 5, 10, 20, 50 or 100</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live price statistics updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/market.html#Subscribe%20to%2024-hour%20Price%20Change" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live price updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/market.html#Subscribe%20to%20Latest%20Trade%20Price" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live best book price updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/market.html#Subscribe%20to%20Best%20Order%20Book" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates. Prior to using this, the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/account.html#Subscription%20order%20update%20data" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string listenKey, Action<DataEvent<BingXOrderUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user balance updates. Prior to using this, the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/account.html#Subscription%20account%20balance%20push" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the <see cref="IBingXRestClientSpotApiAccount.StartUserStreamAsync(CancellationToken)">restClient.SpotApi.Account.StartUserStreamAsync</see> method</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(string listenKey, Action<DataEvent<BingXBalanceUpdate>> onMessage, CancellationToken ct = default);
    }
}
