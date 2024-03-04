using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using BingX.Net.Interfaces.Clients.FuturesApi;
using BingX.Net.Interfaces.Clients.SpotApi;

namespace BingX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BingX Rest API. 
    /// </summary>
    public interface IBingXRestClient : IRestClient
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        IBingXRestClientSpotApi SpotApi { get; }
        /// <summary>
        /// Futures API endpoints
        /// </summary>
        IBingXRestClientFuturesApi FuturesApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
