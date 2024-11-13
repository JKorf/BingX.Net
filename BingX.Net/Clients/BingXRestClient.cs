using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using BingX.Net.Clients.SpotApi;
using CryptoExchange.Net.Clients;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.Apis;
using BingX.Net.Clients.Apis;

namespace BingX.Net.Clients
{
    /// <inheritdoc cref="IBingXRestClient" />
    public class BingXRestClient : BaseRestClient, IBingXRestClient
    {
        #region Api clients

        /// <inheritdoc />
        public IBingXRestClientSpotApi SpotApi { get; }
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesApi PerpetualFuturesApi { get; }
        /// <inheritdoc />
        public IBingXRestClientSubAccountApi SubAccountApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the BingXRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BingXRestClient(Action<BingXRestOptions>? optionsDelegate = null) : this(null, null, optionsDelegate)
        {
        }

        /// <summary>
        /// Create a new instance of the BingXRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public BingXRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, Action<BingXRestOptions>? optionsDelegate = null) : base(loggerFactory, "BingX")
        {
            var options = BingXRestOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            SpotApi = AddApiClient(new BingXRestClientSpotApi(_logger, httpClient, options));
            PerpetualFuturesApi = AddApiClient(new BingXRestClientPerpetualFuturesApi(_logger, httpClient, options));
            SubAccountApi = AddApiClient(new BingXRestClientSubAccountApi(this, _logger, httpClient, options));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BingXRestOptions> optionsDelegate)
        {
            var options = BingXRestOptions.Default.Copy();
            optionsDelegate(options);
            BingXRestOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
            PerpetualFuturesApi.SetApiCredentials(credentials);
        }
    }
}
