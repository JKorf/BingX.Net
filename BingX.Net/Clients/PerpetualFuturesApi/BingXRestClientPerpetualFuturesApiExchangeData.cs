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
    public class BingXRestClientPerpetualFuturesApiExchangeData : IBingXRestClientPerpetualFuturesApiExchangeData
    {
        private readonly ILogger _logger;

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
            var result = await _baseClient.SendRequestInternal<BingXServerTime>(_baseClient.GetUri("/openApi/swap/v2/server/time"), HttpMethod.Get, ct).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        #endregion

        #region Get Contracts

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXContract>>> GetContractsAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<IEnumerable<BingXContract>>(_baseClient.GetUri("/openApi/swap/v2/quote/contracts"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<BingXOrderBook>(_baseClient.GetUri("/openApi/swap/v2/quote/depth"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXFuturesTrade>>(_baseClient.GetUri("/openApi/swap/v2/quote/trades"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesTrade>>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            parameters.AddOptional("fromId", fromId);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXFuturesTrade>>(_baseClient.GetUri("/openApi/swap/v1/market/historicalTrades"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXFundingRate>(_baseClient.GetUri("/openApi/swap/v2/quote/premiumIndex"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFundingRateHistory>>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXFundingRateHistory>>(_baseClient.GetUri("/openApi/swap/v2/quote/fundingRate"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXFuturesKline>>(_baseClient.GetUri("/openApi/swap/v3/quote/klines"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Mark Price Klines

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesMarkPriceKline>>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXFuturesMarkPriceKline>>(_baseClient.GetUri("/openApi/swap/v1/market/markPriceKlines"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXOpenInterest>(_baseClient.GetUri("/openApi/swap/v2/quote/openInterest"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXFuturesTicker>(_baseClient.GetUri("/openApi/swap/v2/quote/ticker"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
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
            var result = await _baseClient.SendRequestInternal<BingXFuturesBookTickerWrapper>(_baseClient.GetUri("/openApi/swap/v2/quote/bookTicker"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result.As<BingXFuturesBookTicker>(result.Data?.BookTicker);
        }

        #endregion

        #region Get Last Trade Price

        /// <inheritdoc />
        public async Task<WebCallResult<BingXLastTradePrice>> GetLastTradePriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            return await _baseClient.SendRequestInternal<BingXLastTradePrice>(_baseClient.GetUri("/openApi/swap/v1/ticker/price"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion
    }
}
