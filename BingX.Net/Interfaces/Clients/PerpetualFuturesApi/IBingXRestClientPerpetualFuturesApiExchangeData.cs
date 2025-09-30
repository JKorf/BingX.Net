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
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#USDT-M%20Perp%20Futures%20symbols" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol name, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXContract[]>> GetContractsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Order%20Book" /></para>
        /// </summary>
        /// <param name="symbol">Symbol name, for example `ETH-USDT`</param>
        /// <param name="limit">Number of rows in the book, 5, 10, 20, 50, 100, 500 or 1000</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get list of the most recent trades
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Recent%20Trades%20List" /></para>
        /// </summary>
        /// <param name="symbol">Symbol name, for example `ETH-USDT`</param>
        /// <param name="limit">Number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get list of trades
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Query%20historical%20transaction%20orders" /></para>
        /// </summary>
        /// <param name="symbol">Symbol name, for example `ETH-USDT`</param>
        /// <param name="fromId">Return trades after this</param>
        /// <param name="limit">Number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesTrade[]>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the current funding rate for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Mark%20Price%20and%20Funding%20Rate" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the current funding rate for all symbols
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Mark%20Price%20and%20Funding%20Rate" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFundingRate[]>> GetFundingRatesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Get%20Funding%20Rate%20History" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFundingRateHistory[]>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Kline/Candlestick%20Data" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open interest
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Open%20Interest%20Statistics" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get ticker (24h price stats) for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#24hr%20Ticker%20Price%20Change%20Statistics" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get ticker (24h price stats) for all symbols
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#24hr%20Ticker%20Price%20Change%20Statistics" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesTicker[]>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the best ask and bid info
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Symbol%20Order%20Book%20Ticker" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get mark price klines
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Mark%20Price%20Kline/Candlestick%20Data" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesMarkPriceKline[]>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade price for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Symbol%20Price%20Ticker" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLastTradePrice>> GetLastTradePriceAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade price for all symbols
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Symbol%20Price%20Ticker" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLastTradePrice[]>> GetLastTradePricesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trading rules for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/market-api.html#Trading%20Rules" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXTradingRules>> GetTradingRulesAsync(string symbol, CancellationToken ct = default);
    }
}
