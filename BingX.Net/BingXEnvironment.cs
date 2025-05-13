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
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public BingXEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        { }

        /// <summary>
        /// Get the BingX environment by name
        /// </summary>
        public static BingXEnvironment? GetEnvironmentByName(string? name)
         => name switch
         {
             TradeEnvironmentNames.Live => Live,
             "demo" => Demo,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name, Demo.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static BingXEnvironment Live { get; }
            = new BingXEnvironment(TradeEnvironmentNames.Live,
                                     BingXApiAddresses.Default.RestClientAddress,
                                     BingXApiAddresses.Default.SocketClientSpotAddress,
                                     BingXApiAddresses.Default.SocketClientSwapAddress);

        /// <summary>
        /// Demo environment. Note that this only applies to trading VST
        /// </summary>
        public static BingXEnvironment Demo { get; }
            = new BingXEnvironment("demo",
                                     BingXApiAddresses.Demo.RestClientAddress,
                                     BingXApiAddresses.Demo.SocketClientSpotAddress,
                                     BingXApiAddresses.Demo.SocketClientSwapAddress);

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
