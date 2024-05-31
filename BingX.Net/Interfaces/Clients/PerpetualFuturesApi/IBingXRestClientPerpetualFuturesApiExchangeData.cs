using BingX.Net.Enums;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Interfaces.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// BingX futures exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBingXRestClientPerpetualFuturesApiExchangeData
    {
        /// <summary>
        /// Get the current server time
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/base-info.html#Get%20Server%20Time" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of contracts
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Contract%20Information" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol name</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXContract>>> GetContractsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Get%20Market%20Depth" /></para>
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <param name="limit">Number of rows in the book, 5, 10, 20, 50, 100, 500 or 1000</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get list of the most recent trades
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#The%20latest%20Trade%20of%20a%20Trading%20Pair" /></para>
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <param name="limit">Number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXFuturesTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get list of trades
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Query%20historical%20transaction%20orders" /></para>
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <param name="fromId">Return trades after this</param>
        /// <param name="limit">Number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXFuturesTrade>>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the current funding rate
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Current%20Funding%20Rate" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Funding%20Rate%20History" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXFundingRateHistory>>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#K-Line%20Data" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXFuturesKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open interest
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Get%20Swap%20Open%20Positions" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get ticker (24h price stats)
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Get%20Ticker" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the best ask and bid info
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Current%20optimal%20listing" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get mark price klines
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#K-Line%20Data%20-%20Mark%20Price" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXFuturesMarkPriceKline>>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade price
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Get%20Latest%20Price%20of%20a%20Trading%20Pair" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLastTradePrice>> GetLastTradePriceAsync(string symbol, CancellationToken ct = default);
    }
}
