using CryptoExchange.Net.Objects;
using BingX.Net.Objects;

namespace BingX.Net
{
    /// <summary>
    /// BingX environments
    /// </summary>
    public class BingXEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest API address
        /// </summary>
        public string RestClientAddress { get; }

        /// <summary>
        /// Socket Spot API address
        /// </summary>
        public string SocketClientSpotAddress { get; }

        /// <summary>
        /// Socket Swap API address
        /// </summary>
        public string SocketClientSwapAddress { get; }

        internal BingXEnvironment(
            string name,
            string restAddress,
            string streamSpotAddress,
            string streamSwapAddress) :
            base(name)
        {
            RestClientAddress = restAddress;
            SocketClientSpotAddress = streamSpotAddress;
            SocketClientSwapAddress = streamSwapAddress;
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static BingXEnvironment Live { get; }
            = new BingXEnvironment(TradeEnvironmentNames.Live,
                                     BingXApiAddresses.Default.RestClientAddress,
                                     BingXApiAddresses.Default.SocketClientSpotAddress,
                                     BingXApiAddresses.Default.SocketClientSwapAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <param name="spotSocketAddress"></param>
        /// <param name="swapSocketAddress"></param>
        /// <returns></returns>
        public static BingXEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketAddress,
                        string swapSocketAddress)
            => new BingXEnvironment(name, spotRestAddress, spotSocketAddress, swapSocketAddress);
    }
}
