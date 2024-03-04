using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using BingX.Net.Objects.Models;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX spot streams
    /// </summary>
    public interface IBingXSocketClientSpotApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// 
        /// <para><a href="BingX" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BingXTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="onMessage"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BingXTickerUpdate>> onMessage, CancellationToken ct = default);
    }
}
