using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using System;
using BingX.Net.Clients.SpotApi;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Clients.PerpetualFuturesApi;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Objects.Options;

namespace BingX.Net.Clients
{
    /// <inheritdoc cref="IBingXSocketClient" />
    public class BingXSocketClient : BaseSocketClient, IBingXSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IBingXSocketClientSpotApi SpotApi { get; set; }

        /// <inheritdoc />
        public IBingXSocketClientPerpetualFuturesApi PerpetualFuturesApi { get; set; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of BingXSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BingXSocketClient(Action<BingXSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of BingXSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public BingXSocketClient(IOptions<BingXSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "BingX")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new BingXSocketClientSpotApi(_logger, options.Value));
            PerpetualFuturesApi = AddApiClient(new BingXSocketClientPerpetualFuturesApi(_logger, options.Value));
        }
        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            PerpetualFuturesApi.SetOptions(options);
            SpotApi.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BingXSocketOptions> optionsDelegate)
        {
            BingXSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
            PerpetualFuturesApi.SetApiCredentials(credentials);
        }
    }
}
