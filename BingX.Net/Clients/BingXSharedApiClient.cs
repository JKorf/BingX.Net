using BingX.Net.Interfaces.Clients;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;

namespace BingX.Net.Clients
{
    /// <inheritdoc />
    public class BingXSharedApiClient : SharedApiClientBase, IBingXSharedApiClient
    {
        /// <inheritdoc />
        public IBingXRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesSharedApi PerpetualFuturesRest { get; }
        /// <inheritdoc />
        public IBingXSocketClientSpotSharedApi SpotSocket { get; }
        /// <inheritdoc />
        public IBingXSocketClientPerpetualFuturesSharedApi PerpetualFuturesSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public BingXSharedApiClient(
            IBingXRestClient restClient,
            IBingXSocketClient socketClient,
            IOptions<BingXOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                  restClient.SpotApi.SharedApi,
                  restClient.PerpetualFuturesApi.SharedApi,
                  socketClient.SpotApi.SharedApi,
                  socketClient.PerpetualFuturesApi.SharedApi
                  )
        {
            SpotRest = restClient.SpotApi.SharedApi;
            PerpetualFuturesRest = restClient.PerpetualFuturesApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
            PerpetualFuturesSocket = socketClient.PerpetualFuturesApi.SharedApi;
        }
    }
}
