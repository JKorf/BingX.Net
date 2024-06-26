﻿using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using System;
using BingX.Net.Clients.SpotApi;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Clients.PerpetualFuturesApi;

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
        /// <param name="loggerFactory">The logger factory</param>
        public BingXSocketClient(ILoggerFactory? loggerFactory = null) : this((x) => { }, loggerFactory)
        {
        }

        /// <summary>
        /// Create a new instance of BingXSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BingXSocketClient(Action<BingXSocketOptions> optionsDelegate) : this(optionsDelegate, null)
        {
        }

        /// <summary>
        /// Create a new instance of BingXSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BingXSocketClient(Action<BingXSocketOptions>? optionsDelegate, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "BingX")
        {
            var options = BingXSocketOptions.Default.Copy();
            optionsDelegate?.Invoke(options);
            Initialize(options);

            SpotApi = AddApiClient(new BingXSocketClientSpotApi(_logger, options));
            PerpetualFuturesApi = AddApiClient(new BingXSocketClientPerpetualFuturesApi(_logger, options));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BingXSocketOptions> optionsDelegate)
        {
            var options = BingXSocketOptions.Default.Copy();
            optionsDelegate(options);
            BingXSocketOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
            PerpetualFuturesApi.SetApiCredentials(credentials);
        }
    }
}
