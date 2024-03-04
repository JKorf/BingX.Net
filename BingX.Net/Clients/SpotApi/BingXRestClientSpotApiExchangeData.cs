using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Internal;

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
            var result = await _baseClient.SendRequestInternal<BingXServerTime>(_baseClient.GetUri("serverTime"), HttpMethod.Get, ct, ignoreRateLimit: true).ConfigureAwait(false);
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
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXTrade>>(_baseClient.GetUri("/openApi/spot/v1/market/trades"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion
    }
}
