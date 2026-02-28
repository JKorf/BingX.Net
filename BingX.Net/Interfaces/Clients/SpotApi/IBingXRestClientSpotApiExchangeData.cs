using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using BingX.Net.Enums;
using BingX.Net.Objects.Models;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBingXRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get the server time
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/base-info.html#Server%20time" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/server/time
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get symbol information
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Spot%20trading%20symbols" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/common/symbols
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of the most recent trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Recent%20Trades%20List" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/market/trades
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the orderbook for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Order%20Book" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/market/depth
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Kline/Candlestick%20Data" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v2/market/kline
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Historical%20K-line" /><br />
        /// Endpoint:<br />
        /// GET /openApi/market/his/v1/kline
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXKline[]>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get ticker (24h price statistics)
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#24hr%20Ticker%20Price%20Change%20Statistics" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/ticker/24hr
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get aggregated order book
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Order%20Book%20aggregation" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v2/market/depth
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="limit">Book depth</param>
        /// <param name="mergeDepth">0 is default precision, 1 to 5 are 10 to 100000 times precision respectively</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrderBook>> GetAggregatedOrderBookAsync(string symbol, int limit, int mergeDepth, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Symbol%20Price%20Ticker" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/ticker/price
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLastTrade>> GetLastTradeAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Symbol%20Price%20Ticker" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/ticker/price
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLastTrade[]>> GetLastTradesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the current best book prices
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Symbol%20Order%20Book%20Ticker" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/ticker/bookTicker
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get historic trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/market-api.html#Old%20Trade%20Lookup" /><br />
        /// Endpoint:<br />
        /// GET /openApi/market/his/v1/trade
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="limit">Amount or results, max 500</param>
        /// <param name="fromId">Return trades after this id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXTradeHistory[]>> GetTradeHistoryAsync(string symbol, int? limit = null, string? fromId = null, CancellationToken ct = default);
    }
}
