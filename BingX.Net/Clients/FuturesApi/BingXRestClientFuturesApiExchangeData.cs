using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BingX.Net.Interfaces.Clients.FuturesApi;
using BingX.Net.Objects.Models;

namespace BingX.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    public class BingXRestClientFuturesApiExchangeData : IBingXRestClientFuturesApiExchangeData
    {
        private readonly ILogger _logger;

        private readonly BingXRestClientFuturesApi _baseClient;

        internal BingXRestClientFuturesApiExchangeData(ILogger logger, BingXRestClientFuturesApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }


        #region Get Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternal<BingXModel>(_baseClient.GetUri("BingX"), HttpMethod.Get, ct, ignoreRateLimit: true).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        #endregion
    }
}
