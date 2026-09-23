using BingX.Net.Interfaces.Clients;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;

namespace BingX.Net
{
    /// <inheritdoc />
    public class BingXUserSpotDataTracker : UserSpotDataTracker
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BingXUserSpotDataTracker(
            ILogger<BingXUserSpotDataTracker> logger,
            IBingXRestClient restClient,
            IBingXSocketClient socketClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig? config) : base(
                logger,
                restClient.SpotApi.SharedApi,
                restClient.SpotApi.SharedApi,
                socketClient.SpotApi.SharedApi,

                restClient.SpotApi.SharedApi,
                restClient.SpotApi.SharedApi,
                socketClient.SpotApi.SharedApi,

                restClient.SpotApi.SharedApi,
                null,
                userIdentifier,
                config ?? new SpotUserDataTrackerConfig())
        {

        }
    }

    /// <inheritdoc />
    public class BingXUserPerpetualFuturesDataTracker : UserFuturesDataTracker
    {
        /// <inheritdoc />
        protected override bool WebsocketPositionUpdatesAreFullSnapshots => false;

        /// <summary>
        /// ctor
        /// </summary>
        public BingXUserPerpetualFuturesDataTracker(
            ILogger<BingXUserPerpetualFuturesDataTracker> logger,
            IBingXRestClient restClient,
            IBingXSocketClient socketClient,
            string? userIdentifier,
            FuturesUserDataTrackerConfig? config) : base(logger,
                restClient.PerpetualFuturesApi.SharedApi,
                restClient.PerpetualFuturesApi.SharedApi,
                socketClient.PerpetualFuturesApi.SharedApi,

                restClient.PerpetualFuturesApi.SharedApi,
                restClient.PerpetualFuturesApi.SharedApi,
                socketClient.PerpetualFuturesApi.SharedApi,

                restClient.PerpetualFuturesApi.SharedApi,
                null,

                restClient.PerpetualFuturesApi.SharedApi,
                socketClient.PerpetualFuturesApi.SharedApi,
                userIdentifier,
                config ?? new FuturesUserDataTrackerConfig())
        {

        }
    }
}
