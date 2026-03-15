using CryptoExchange.Net.Authentication;
using System;

namespace BingX.Net
{
    /// <summary>
    /// BingX API credentials
    /// </summary>
    public class BingXCredentials : ApiCredentials
    {
        /// <summary>
        /// </summary>
        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use parameterized constructor instead.")]
        public BingXCredentials() { }

        /// <summary>
        /// Create credentials using an HMAC key and secret.
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="secret">API secret</param>
        public BingXCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }
       
        /// <summary>
        /// Create credentials using HMAC credentials
        /// </summary>
        /// <param name="credential">HMAC credentials</param>
        public BingXCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
        public override ApiCredentials Copy() => new BingXCredentials { CredentialPairs = CredentialPairs };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
