using CryptoExchange.Net.Objects.Options;
using System;

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
        /// ctor
        /// </summary>
        public BingXRestOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// The receive window
        /// </summary>
        public TimeSpan? ReceiveWindow { get; set; }

        /// <summary>
        /// Broker id
        /// </summary>
        public string? BrokerId { get; set; }

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

        internal BingXRestOptions Set(BingXRestOptions targetOptions)
        {
            targetOptions = base.Set<BingXRestOptions>(targetOptions);
            targetOptions.ReceiveWindow = ReceiveWindow;
            targetOptions.BrokerId = BrokerId;
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            return targetOptions;
        }
    }
}
