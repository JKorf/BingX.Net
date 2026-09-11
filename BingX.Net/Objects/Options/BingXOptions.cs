using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// BingX services options
    /// </summary>
    public class BingXOptions : LibraryOptions<BingXRestOptions, BingXSocketOptions, BingXCredentials, BingXEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
