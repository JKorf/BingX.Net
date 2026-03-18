using CryptoExchange.Net.Authentication;

namespace BingX.Net
{
    /// <summary>
    /// BingX API credentials
    /// </summary>
    public class BingXCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials providing only credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public BingXCredentials(string key, string secret) : base(key, secret)
        {
        }
    }
}
