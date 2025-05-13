using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using CryptoExchange.Net.Objects.Options;

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
        /// <see cref="IBingXSocketClientSpotApi"/>
        IBingXSocketClientSpotApi SpotApi { get; }
        /// <summary>
        /// Perpetual Futures streams
        /// </summary>
        /// <see cref="IBingXSocketClientPerpetualFuturesApi"/>
        IBingXSocketClientPerpetualFuturesApi PerpetualFuturesApi { get; }

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
