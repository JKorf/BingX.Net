using CryptoExchange.Net.Objects.Options;
using System;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// Options for the BingXSocketClient
    /// </summary>
    public class BingXSocketOptions : SocketExchangeOptions<BingXEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static BingXSocketOptions Default { get; set; } = new BingXSocketOptions()
        {
            Environment = BingXEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10,
        };

        /// <summary>
        /// ctor
        /// </summary>
        public BingXSocketOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions()
        {
            SocketNoDataTimeout = TimeSpan.FromSeconds(40) // Ping message is sent every 30 seconds
        };

        /// <summary>
        /// Options for the Futures API
        /// </summary>
        public SocketApiOptions FuturesOptions { get; private set; } = new SocketApiOptions()
        {
            MaxSocketConnections = 60,
            SocketNoDataTimeout = TimeSpan.FromSeconds(40) // Ping message is sent every 30 seconds
        };

        internal BingXSocketOptions Set(BingXSocketOptions targetOptions)
        {
            targetOptions = base.Set<BingXSocketOptions>(targetOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            return targetOptions;
        }
    }
}
