using BingX.Net.Interfaces.Clients;
using CryptoExchange.Net.Trackers.UserData;
using Microsoft.Extensions.Logging;

namespace BingX.Net
{
    public class BingXUserSpotDataTracker : UserSpotDataTracker
    {
        public BingXUserSpotDataTracker(
            ILogger<BingXUserSpotDataTracker> logger,
            IBingXRestClient restClient,
            IBingXSocketClient socketClient,
            string? userIdentifier,
            UserDataTrackerConfig config) : base(logger, restClient.SpotApi.SharedClient, socketClient.SpotApi.SharedClient, userIdentifier, config)
        {

        }
    }

    public class BingXUserFuturesDataTracker : UserFuturesDataTracker
    {
        protected override bool WebsocketPositionUpdatesAreFullSnapshots => false;

        public BingXUserFuturesDataTracker(
            ILogger<BingXUserFuturesDataTracker> logger,
            IBingXRestClient restClient,
            IBingXSocketClient socketClient,
            string? userIdentifier,
            UserDataTrackerConfig config) : base(logger, restClient.PerpetualFuturesApi.SharedClient, socketClient.PerpetualFuturesApi.SharedClient, userIdentifier, config)
        {

        }
    }
}
