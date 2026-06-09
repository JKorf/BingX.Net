using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net;
using System.Linq;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class BingXRestClientSpotApiExchangeData : IBingXRestClientSpotApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientSpotApi _baseClient;

        internal BingXRestClientSpotApiExchangeData(ILogger logger, BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/server/time", BingXExchange.RateLimiter.RestMarket, 1, false, preventCaching: true);
            var result = await _baseClient.SendAsync<BingXServerTime>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, result.Data.ServerTime);
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<BingXSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/common/symbols", BingXExchange.RateLimiter.RestMarket, 1, false);
            var result =  await _baseClient.SendAsync<BingXSymbolsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXSymbol[]>(result);

            return HttpResult.Ok(result, result.Data.Symbols);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<BingXTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/market/trades", BingXExchange.RateLimiter.RestMarket, 1, false);
            return await _baseClient.SendAsync<BingXTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/market/depth", BingXExchange.RateLimiter.RestMarket, 1, false);
            return await _baseClient.SendAsync<BingXOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Aggregated Order Book

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrderBook>> GetAggregatedOrderBookAsync(string symbol, int limit, int mergeDepth, CancellationToken ct = default)
        {
            mergeDepth.ValidateIntBetween(nameof(mergeDepth), 0, 5);

            // This endpoint doesn't work unless symbol is with an underscore
            symbol = symbol.Replace('-', '_');

            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol },
                { "depth", limit },
                { "type", "step" + mergeDepth }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v2/market/depth", BingXExchange.RateLimiter.RestMarket, 1, false);
            return await _baseClient.SendAsync<BingXOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<BingXKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol },
                { "interval", EnumConverter.GetString(interval) }
            };
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v2/market/kline", BingXExchange.RateLimiter.RestMarket, 1, false);
            return await _baseClient.SendAsync<BingXKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<BingXKline[]>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol },
                { "interval", EnumConverter.GetString(interval) }
            };
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/market/his/v1/kline", BingXExchange.RateLimiter.RestMarket, 1, false);
            return await _baseClient.SendAsync<BingXKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<BingXTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "timestamp", DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow) }
            };
            parameters.Add("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/ticker/24hr", BingXExchange.RateLimiter.RestMarket, 1, false);
            return await _baseClient.SendAsync<BingXTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade

        /// <inheritdoc />
        public async Task<HttpResult<BingXLastTrade>> GetLastTradeAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/ticker/price", BingXExchange.RateLimiter.RestMarket, 1, false);
            var result = await _baseClient.SendAsync<BingXLastTradeWrapper[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXLastTrade>(result);

            var tradeResult = HttpResult.Ok(result, result.Data.Single().Trades.Single());
            tradeResult.Data!.Symbol = symbol;
            return tradeResult;
        }

        #endregion

        #region Get Last Trades

        /// <inheritdoc />
        public async Task<HttpResult<BingXLastTrade[]>> GetLastTradesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/ticker/price", BingXExchange.RateLimiter.RestMarket, 1, false);
            var result = await _baseClient.SendAsync<BingXLastTradeWrapper[]>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXLastTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x => x.Trades.Single() with { Symbol = x.Symbol }).ToArray());
        }

        #endregion

        #region Get Book Price

        /// <inheritdoc />
        public async Task<HttpResult<BingXBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/ticker/bookTicker", BingXExchange.RateLimiter.RestMarket, 1, false);
            var result = await _baseClient.SendAsync<BingXBookTicker[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXBookTicker>(result);

            return HttpResult.Ok(result, result.Data.Single());
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<HttpResult<BingXTradeHistory[]>> GetTradeHistoryAsync(string symbol, int? limit = null, string? fromId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("limit", limit);
            parameters.Add("fromId", fromId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/market/his/v1/trade", BingXExchange.RateLimiter.RestMarket, 1, false);
            return await _baseClient.SendAsync<BingXTradeHistory[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
