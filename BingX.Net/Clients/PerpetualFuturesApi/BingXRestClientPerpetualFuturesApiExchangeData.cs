using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BingX.Net.Objects.Internal;
using System.Net.Http;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using System.Collections.Generic;
using BingX.Net.Objects.Models;
using BingX.Net.Enums;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    internal class BingXRestClientPerpetualFuturesApiExchangeData : IBingXRestClientPerpetualFuturesApiExchangeData
    {
        private readonly ILogger _logger;

        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientPerpetualFuturesApi _baseClient;

        internal BingXRestClientPerpetualFuturesApiExchangeData(ILogger logger, BingXRestClientPerpetualFuturesApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/server/time", BingXExchange.RateLimiter.RestMarket, 1, false, preventCaching: true);
            var result = await _baseClient.SendAsync<BingXServerTime>(request, null, ct).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        #endregion

        #region Get Contracts

        /// <inheritdoc />
        public async Task<WebCallResult<BingXContract[]>> GetContractsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/contracts", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXContract[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/depth", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/trades", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesTrade[]>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            parameters.AddOptional("fromId", fromId);
            parameters.AddOptionalMillisecondsString("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/market/historicalTrades", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rate

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/premiumIndex", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFundingRate>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rates

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFundingRate[]>> GetFundingRatesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/premiumIndex", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFundingRate[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFundingRateHistory[]>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/fundingRate", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFundingRateHistory[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v3/quote/klines", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Mark Price Klines

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesMarkPriceKline[]>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);
            parameters.AddOptionalMillisecondsString("timestamp", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/market/markPriceKlines", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesMarkPriceKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Interest

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/openInterest", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXOpenInterest>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/ticker", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTicker>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesTicker[]>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/ticker", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTicker[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptionalMillisecondsString("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/quote/bookTicker", BingXExchange.RateLimiter.RestMarket, 1);
            var result = await _baseClient.SendAsync<BingXFuturesBookTickerWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesBookTicker>(result.Data?.BookTicker);
        }

        #endregion

        #region Get Last Trade Price

        /// <inheritdoc />
        public async Task<WebCallResult<BingXLastTradePrice>> GetLastTradePriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddOptionalMillisecondsString("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/ticker/price", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXLastTradePrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Prices

        /// <inheritdoc />
        public async Task<WebCallResult<BingXLastTradePrice[]>> GetLastTradePricesAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalMillisecondsString("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/ticker/price", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXLastTradePrice[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trading Rules

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTradingRules>> GetTradingRulesAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddOptionalMillisecondsString("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/tradingRules", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXTradingRules>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
