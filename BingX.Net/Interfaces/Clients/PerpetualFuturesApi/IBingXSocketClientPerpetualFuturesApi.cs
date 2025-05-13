using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace BingX.Net.Interfaces.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// BingX futures streams
    /// </summary>
    public interface IBingXSocketClientPerpetualFuturesApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        IBingXSocketClientPerpetualFuturesApiShared SharedClient { get; }

        /// <summary>
        /// Subscribe to live trade updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20the%20Latest%20Trade%20Detail" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTradeUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20Market%20Depth%20Data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="depth">Book depth, 5, 10, 20, 50 or 100</param>
        /// <param name="updateInterval">The update interval in ms, 100, 200, 500 or 1000</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates for all symbols
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20Market%20Depth%20Data%20of%20all%20trading%20pairs" /></para>
        /// </summary>
        /// <param name="depth">Book depth, 5, 10, 20, 50 or 100</param>
        /// <param name="updateInterval">The update interval in ms, 100, 200, 500 or 1000</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(int depth, int updateInterval, Action<DataEvent<BingXOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20K-Line%20Data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick updates for all symbols
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20K-Line%20Data" /></para>
        /// </summary>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(KlineInterval interval, Action<DataEvent<BingXFuturesKlineUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live price statistics updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20K-Line%20Data%20of%20all%20trading%20pairs" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live price statistics updates for all symbols
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20to%2024-hour%20price%20changes%20of%20all%20trading%20pairs" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BingXFuturesTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live price updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20to%20latest%20price%20changes" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(string symbol, Action<DataEvent<BingXPriceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live mark price updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20to%20latest%20mark%20price%20changes" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<BingXMarkPriceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live best book price updates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/market.html#Subscribe%20to%20the%20Book%20Ticker%20Streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookPriceUpdatesAsync(string symbol, Action<DataEvent<BingXBookTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Listen to user data update events. Prior to using this, the <see cref="IBingXRestClientPerpetualFuturesApiAccount.StartUserStreamAsync(CancellationToken)">restClient.PerpetualFuturesApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/account.html#listenKey%20expired%20push" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the <see cref="IBingXRestClientPerpetualFuturesApiAccount.StartUserStreamAsync(CancellationToken)">restClient.PerpetualFuturesApi.Account.StartUserStreamAsync</see> method</param>
        /// <param name="onAccountUpdate">Event handler for balance and position updates</param>
        /// <param name="onOrderUpdate">Event handler for order updates</param>
        /// <param name="onConfigurationUpdate">Event handler for account configuration updates</param>
        /// <param name="onListenKeyExpiredUpdate">Event handler for listenkey expired event</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<BingXFuturesAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<BingXFuturesOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<BingXConfigUpdate>>? onConfigurationUpdate = null,
            Action<DataEvent<BingXListenKeyExpiredUpdate>>? onListenKeyExpiredUpdate = null,
            CancellationToken ct = default);
    }
}
