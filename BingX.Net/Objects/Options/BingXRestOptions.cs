using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// Options for the BingXRestClient
    /// </summary>
    public class BingXRestOptions : RestExchangeOptions<BingXEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static BingXRestOptions Default { get; set; } = new BingXRestOptions()
        {
            Environment = BingXEnvironment.Live,
            AutoTimestamp = false
        };

        /// <summary>
        /// The receive window
        /// </summary>
        public TimeSpan? ReceiveWindow { get; set; }

        /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions
        {
        };

        /// <summary>
        /// Futures API options
        /// </summary>
        public RestApiOptions FuturesOptions { get; private set; } = new RestApiOptions();

        internal BingXRestOptions Copy()
        {
            var options = Copy<BingXRestOptions>();
            options.ReceiveWindow = ReceiveWindow;
            options.SpotOptions = SpotOptions.Copy<RestApiOptions>();
            options.FuturesOptions = FuturesOptions.Copy<RestApiOptions>();
            return options;
        }
    }
}
