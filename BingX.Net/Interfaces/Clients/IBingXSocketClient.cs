using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;

namespace BingX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BingX websocket API
    /// </summary>
    public interface IBingXSocketClient : ISocketClient
    {
        /// <summary>
        /// Spot streams
        /// </summary>
        IBingXSocketClientSpotApi SpotApi { get; }
        /// <summary>
        /// Perpetual Futures streams
        /// </summary>
        IBingXSocketClientPerpetualFuturesApi PerpetualFuturesApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
