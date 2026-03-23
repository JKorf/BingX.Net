using CryptoExchange.Net.Authentication;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.Interfaces.Clients;

namespace BingX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BingX websocket API
    /// </summary>
    public interface IBingXSocketClient : ISocketClient<BingXCredentials>
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
    }
}
