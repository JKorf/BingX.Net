using CryptoExchange.Net.Interfaces;
using System;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace BingX.Net.Interfaces
{
    /// <summary>
    /// BingX local order book factory
    /// </summary>
    public interface IBingXOrderBookFactory
    {
        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        public IOrderBookFactory<BingXOrderBookOptions> Spot { get; }
        /// <summary>
        /// Perpetual Futures order book factory methods
        /// </summary>
        public IOrderBookFactory<BingXOrderBookOptions> PerpetualFutures { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<BingXOrderBookOptions>? options = null);

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