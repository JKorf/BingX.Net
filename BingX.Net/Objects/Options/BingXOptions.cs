using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// BingX services options
    /// </summary>
    public class BingXOptions : LibraryOptions<BingXRestOptions, BingXSocketOptions, ApiCredentials, BingXEnvironment>
    {
    }
}
