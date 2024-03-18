using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BingX.Net.Objects.Internal;
using System.Net.Http;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;

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
            var result = await _baseClient.SendRequestInternal<BingXServerTime>(_baseClient.GetUri("/openApi/swap/v2/server/time"), HttpMethod.Get, ct, ignoreRateLimit: true).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        #endregion
    }
}
