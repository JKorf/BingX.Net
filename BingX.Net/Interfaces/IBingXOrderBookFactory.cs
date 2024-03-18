using CryptoExchange.Net.Interfaces;
using System;
using BingX.Net.Objects.Options;

namespace BingX.Net.Interfaces
{
    /// <summary>
    /// BingX local order book factory
    /// </summary>
    public interface IBingXOrderBookFactory
    {
        /// <summary>
        /// Create a new futures local order book instance
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        ISymbolOrderBook CreatePerpetualFutures(string symbol, Action<BingXOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new spot local order book instance
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        ISymbolOrderBook CreateSpot(string symbol, Action<BingXOrderBookOptions>? options = null);
    }
}