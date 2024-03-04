using CryptoExchange.Net.Objects.Options;

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
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions()
        {
        };

        /// <summary>
        /// Options for the Futures API
        /// </summary>
        public SocketApiOptions FuturesOptions { get; private set; } = new SocketApiOptions();

        internal BingXSocketOptions Copy()
        {
            var options = Copy<BingXSocketOptions>();
            options.SpotOptions = SpotOptions.Copy<SocketApiOptions>();
            options.FuturesOptions = FuturesOptions.Copy<SocketApiOptions>();
            return options;
        }
    }
}
