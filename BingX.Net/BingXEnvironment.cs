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
        /// Socket API address
        /// </summary>
        public string SocketClientAddress { get; }

        internal BingXEnvironment(
            string name,
            string restAddress,
            string streamAddress) :
            base(name)
        {
            RestClientAddress = restAddress;
            SocketClientAddress = streamAddress;
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static BingXEnvironment Live { get; }
            = new BingXEnvironment(TradeEnvironmentNames.Live,
                                     BingXApiAddresses.Default.RestClientAddress,
                                     BingXApiAddresses.Default.SocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <param name="spotSocketStreamsAddress"></param>
        /// <returns></returns>
        public static BingXEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketStreamsAddress)
            => new BingXEnvironment(name, spotRestAddress, spotSocketStreamsAddress);
    }
}
