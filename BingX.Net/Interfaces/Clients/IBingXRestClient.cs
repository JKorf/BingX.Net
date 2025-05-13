using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.Apis;
using CryptoExchange.Net.Objects.Options;

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
        /// <see cref="IBingXRestClientSpotApi"/>
        IBingXRestClientSpotApi SpotApi { get; }
        /// <summary>
        /// Perpetual Futures API endpoints
        /// </summary>
        /// <see cref="IBingXRestClientPerpetualFuturesApi"/>
        IBingXRestClientPerpetualFuturesApi PerpetualFuturesApi { get; }
        /// <summary>
        /// Sub account API endpoints
        /// </summary>
        /// <see cref="IBingXRestClientSubAccountApi"/>
        IBingXRestClientSubAccountApi SubAccountApi { get; }

        /// <summary>
        /// Update specific options
        /// </summary>
        /// <param name="options">Options to update. Only specific options are changeable after the client has been created</param>
        void SetOptions(UpdateOptions options);

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
