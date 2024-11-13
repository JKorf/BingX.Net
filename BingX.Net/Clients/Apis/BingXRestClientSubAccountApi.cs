using BingX.Net.Clients.SpotApi;
using BingX.Net.Interfaces.Clients.Apis;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.Apis
{
    /// <inheritdoc cref="IBingXRestClientSubAccountApi" />
    public class BingXRestClientSubAccountApi : BingXRestClientApi, IBingXRestClientSubAccountApi
    {
        #region fields 
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClient _baseClient;
        #endregion

        #region constructor/destructor
        internal BingXRestClientSubAccountApi(BingXRestClient baseClient, ILogger logger, HttpClient? httpClient, BingXRestOptions options)
            : base(logger, httpClient, options, options.SpotOptions)
        {
            _baseClient = baseClient;
        }
        #endregion

        #region Get Api Key Permissions

        /// <inheritdoc />
        public async Task<WebCallResult<BingXKeyPermissions>> GetApiKeyPermissionsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/v1/account/apiPermissions", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await SendRawAsync<BingXKeyPermissions>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => _baseClient.SpotApi.ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => ((BingXRestClientSpotApi)_baseClient.SpotApi).GetTimeSyncInfo();

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => ((BingXRestClientSpotApi)_baseClient.SpotApi).GetTimeOffset();
    }
}
