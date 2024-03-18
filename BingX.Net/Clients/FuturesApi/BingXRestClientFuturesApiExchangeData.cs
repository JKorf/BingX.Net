using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.FuturesApi;

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
            throw new NotImplementedException();
        }

        #endregion
    }
}
