using BingX.Net.Enums;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/USDT-M%20Perp%20Futures%20symbols" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/server/time
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of contracts
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/USDT-M%20Perp%20Futures%20symbols" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/contracts
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol name, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXContract[]>> GetContractsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Order%20Book" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/depth
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol name, for example `ETH-USDT`</param>
        /// <param name="limit">["<c>limit</c>"] Number of rows in the book, 5, 10, 20, 50, 100, 500 or 1000</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get list of the most recent trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Recent%20Trades%20List" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/trades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol name, for example `ETH-USDT`</param>
        /// <param name="limit">["<c>limit</c>"] Number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get list of trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Query%20historical%20transaction%20orders" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/market/historicalTrades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol name, for example `ETH-USDT`</param>
        /// <param name="fromId">["<c>fromId</c>"] Return trades after this</param>
        /// <param name="limit">["<c>limit</c>"] Number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesTrade[]>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the current funding rate for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Mark%20Price%20and%20Funding%20Rate" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/premiumIndex
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the current funding rate for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Mark%20Price%20and%20Funding%20Rate" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/premiumIndex
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFundingRate[]>> GetFundingRatesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Get%20Funding%20Rate%20History" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/fundingRate
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFundingRateHistory[]>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Kline%2FCandlestick%20Data" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v3/quote/klines
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="interval">["<c>interval</c>"] Kline interval</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open interest
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Open%20Interest%20Statistics" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/openInterest
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get ticker (24h price stats) for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/24hr%20Ticker%20Price%20Change%20Statistics" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/ticker
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get ticker (24h price stats) for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/24hr%20Ticker%20Price%20Change%20Statistics" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/ticker
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesTicker[]>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the best ask and bid info
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Symbol%20Order%20Book%20Ticker" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/quote/bookTicker
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get mark price klines
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Mark%20Price%20Kline%2FCandlestick%20Data" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/market/markPriceKlines
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="interval">["<c>interval</c>"] Kline interval</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXFuturesMarkPriceKline[]>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade price for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Symbol%20Price%20Ticker" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/ticker/price
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXLastTradePrice>> GetLastTradePriceAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade price for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Symbol%20Price%20Ticker" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/ticker/price
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BingXLastTradePrice[]>> GetLastTradePricesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trading rules for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs-v3/#/en/Swap/Market%20Data/Trading%20Rules" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/tradingRules
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BingXTradingRules>> GetTradingRulesAsync(string symbol, CancellationToken ct = default);
    }
}
