using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using BingX.Net.Interfaces.Clients.FuturesApi;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Clients;

namespace BingX.Net.Clients.FuturesApi
{
    /// <inheritdoc cref="IBingXRestClientFuturesApi" />
    public class BingXRestClientFuturesApi : RestApiClient, IBingXRestClientFuturesApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("USD Futures Api");
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBingXRestClientFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public IBingXRestClientFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBingXRestClientFuturesApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "BingX";
        #endregion

        #region constructor/destructor
        internal BingXRestClientFuturesApi(ILogger logger, HttpClient? httpClient, BingXRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress!, options, options.FuturesOptions)
        {
            Account = new BingXRestClientFuturesApiAccount(this);
            ExchangeData = new BingXRestClientFuturesApiExchangeData(logger, this);
            Trading = new BingXRestClientFuturesApiTrading(logger, this);
        }

        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        internal Uri GetUri(string path) => new Uri(BaseAddress.AppendPath(path));

        internal async Task<WebCallResult<T>> SendRequestInternal<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        {
            return await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, null, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp, ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;
    }
}
