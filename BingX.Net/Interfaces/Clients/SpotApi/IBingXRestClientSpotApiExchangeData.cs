using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using BingX.Net.Enums;
using BingX.Net.Objects.Models;
using System.Collections.Generic;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBingXRestClientSpotApiExchangeData
    {
        /// <summary>
        /// 
        /// <para><a href="BingX" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXSymbol>>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="limit"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="limit"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="limit"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get ticker (24h price statistics)
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#24-hour%20price%20changes" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXTicker>>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get aggregated order book
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Query%20Aggregate%20Depth" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="limit">Book depth</param>
        /// <param name="mergeDepth">0 is default precision, 1 to 5 are 10 to 100000 times precision respectively</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrderBook>> GetAggregatedOrderBookAsync(string symbol, int limit, int mergeDepth, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Latest%20Transaction%20Price" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLastTrade>> GetLastTradeAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the current best book prices
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Best%20Order%20Book" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default);
    }
}
