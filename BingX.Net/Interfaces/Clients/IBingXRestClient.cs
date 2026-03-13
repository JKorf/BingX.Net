using CryptoExchange.Net.Authentication;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.Apis;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.Interfaces.Clients;

namespace BingX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BingX Rest API. 
    /// </summary>
    public interface IBingXRestClient : IRestClient<BingXCredentials>
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
    }
}
