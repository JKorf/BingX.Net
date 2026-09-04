using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.SpotApi;

namespace BingX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of BingX
    /// </summary>
    public interface IBingXSharedApiClient
    {
        /// <summary>
        /// Spot REST shared API implementations
        /// </summary>
        IBingXRestClientSpotSharedApi SpotRest { get; }

        /// <summary>
        /// Futures REST shared API implementations
        /// </summary>
        IBingXRestClientPerpetualFuturesSharedApi FuturesRest { get; }

        /// <summary>
        /// Spot WebSocket shared API implementations
        /// </summary>
        IBingXSocketClientSpotSharedApi SpotSocket { get; }

        /// <summary>
        /// Futures WebSocket shared API implementations
        /// </summary>
        IBingXSocketClientPerpetualFuturesSharedApi FuturesSocket { get; }
    }
}
