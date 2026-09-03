using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BingX.Net.Objects.Internal;
using System.Net.Http;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using CryptoExchange.Net.Objects.Errors;

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
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/server/time", BingXExchange.RateLimiter.RestMarket, 1, false, preventCaching: true);
            var result = await _baseClient.SendAsync<BingXServerTime>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, result.Data.ServerTime);
        }

        #endregion

        #region Get Contracts

        /// <inheritdoc />
        public async Task<HttpResult<BingXContract[]>> GetContractsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/contracts", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXContract[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/depth", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/trades", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesTrade[]>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("limit", limit);
            parameters.Add("fromId", fromId);
            parameters.Add("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/market/historicalTrades", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rate

        /// <inheritdoc />
        public async Task<HttpResult<BingXFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/premiumIndex", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFundingRate>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rates

        /// <inheritdoc />
        public async Task<HttpResult<BingXFundingRate[]>> GetFundingRatesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/premiumIndex", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFundingRate[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<HttpResult<BingXFundingRateHistory[]>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/fundingRate", BingXExchange.RateLimiter.RestMarket, 1);
            var result = await _baseClient.SendAsync<BingXFundingRateHistory[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data == null)
                return HttpResult.Ok<BingXFundingRateHistory[]>(result, []);

            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("interval", interval);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v3/quote/klines", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Mark Price Klines

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesMarkPriceKline[]>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("interval", interval);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            parameters.Add("timestamp", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/market/markPriceKlines", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesMarkPriceKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Interest

        /// <inheritdoc />
        public async Task<HttpResult<BingXOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/openInterest", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXOpenInterest>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/ticker", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTicker>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesTicker[]>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/ticker", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXFuturesTicker[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Ticker

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/quote/bookTicker", BingXExchange.RateLimiter.RestMarket, 1);
            var result = await _baseClient.SendAsync<BingXFuturesBookTickerWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesBookTicker>(result);

            return HttpResult.Ok(result, result.Data.BookTicker);
        }

        #endregion

        #region Get Last Trade Price

        /// <inheritdoc />
        public async Task<HttpResult<BingXLastTradePrice>> GetLastTradePriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/ticker/price", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXLastTradePrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Prices

        /// <inheritdoc />
        public async Task<HttpResult<BingXLastTradePrice[]>> GetLastTradePricesAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/ticker/price", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXLastTradePrice[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trading Rules

        /// <inheritdoc />
        public async Task<HttpResult<BingXTradingRules>> GetTradingRulesAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("timestamp", DateTime.UtcNow);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/tradingRules", BingXExchange.RateLimiter.RestMarket, 1);
            return await _baseClient.SendAsync<BingXTradingRules>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
