using System;
using System.Collections.Generic;
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
    public class BingXRestClientSpotApiExchangeData : IBingXRestClientSpotApiExchangeData
    {
        private readonly BingXRestClientSpotApi _baseClient;

        internal BingXRestClientSpotApiExchangeData(ILogger logger, BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternal<BingXServerTime>(_baseClient.GetUri("/openApi/spot/v1/server/time"), HttpMethod.Get, ct).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXSymbol>>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            var result = await _baseClient.SendRequestInternal<BingXSymbolsWrapper>(_baseClient.GetUri("/openApi/spot/v1/common/symbols"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            return result.As<IEnumerable<BingXSymbol>>(result.Data?.Symbols);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXTrade>>(_baseClient.GetUri("/openApi/spot/v1/market/trades"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<BingXOrderBook>(_baseClient.GetUri("/openApi/spot/v1/market/depth"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Aggregated Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrderBook>> GetAggregatedOrderBookAsync(string symbol, int limit, int mergeDepth, CancellationToken ct = default)
        {
            mergeDepth.ValidateIntBetween(nameof(mergeDepth), 0, 5);

            // This endpoint doesn't work unless symbol is with an underscore
            symbol = symbol.Replace('-', '_');

            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "depth", limit },
                { "type", "step" + mergeDepth }
            };
            return await _baseClient.SendRequestInternal<BingXOrderBook>(_baseClient.GetUri("/openApi/spot/v2/market/depth"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "interval", EnumConverter.GetString(interval) }
            };
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXKline>>(_baseClient.GetUri("/openApi/spot/v1/market/kline"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXTicker>>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "timestamp", DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow) }
            };
            parameters.AddOptional("symbol", symbol);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXTicker>>(_baseClient.GetUri("/openApi/spot/v1/ticker/24hr"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade

        /// <inheritdoc />
        public async Task<WebCallResult<BingXLastTrade>> GetLastTradeAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            var result = await _baseClient.SendRequestInternal<IEnumerable<BingXLastTradeWrapper>>(_baseClient.GetUri("/openApi/spot/v1/ticker/price"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            return result.As<BingXLastTrade>(result.Data?.Single().Trades.Single());
        }

        #endregion

        #region Get Book Price

        /// <inheritdoc />
        public async Task<WebCallResult<BingXBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            var result = await _baseClient.SendRequestInternal<IEnumerable<BingXBookTicker>>(_baseClient.GetUri("/openApi/spot/v1/ticker/bookTicker"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            return result.As<BingXBookTicker>(result.Data?.Single());
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, string? fromId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            parameters.AddOptional("fromId", fromId);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXTrade>>(_baseClient.GetUri("/openApi/spot/v1/market/trades"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion
    }
}
