using BingX.Net.Interfaces.Clients;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.SpotApi;

namespace BingX.Net.Clients
{
    /// <inheritdoc />
    public class BingXSharedApiClient : IBingXSharedApiClient
    {
        /// <inheritdoc />
        public IBingXRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesSharedApi FuturesRest { get; }
        /// <inheritdoc />
        public IBingXSocketClientSpotSharedApi SpotSocket { get; }
        /// <inheritdoc />
        public IBingXSocketClientPerpetualFuturesSharedApi FuturesSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public BingXSharedApiClient(
            IBingXRestClient restClient,
            IBingXSocketClient socketClient)
        {
            SpotRest = restClient.SpotApi.SharedApi;
            FuturesRest = restClient.PerpetualFuturesApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
            FuturesSocket = socketClient.PerpetualFuturesApi.SharedApi;
        }
    }
}
