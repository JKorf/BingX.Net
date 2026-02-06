using BingX.Net.Clients;
using BingX.Net.Interfaces;
using BingX.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace BingX.Net
{
    /// <inheritdoc />
    public class BingXTrackerFactory : IBingXTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public BingXTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BingXTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval) => true;

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBingXRestClient>() ?? new BingXRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBingXSocketClient>() ?? new BingXSocketClient();

            IKlineRestClient sharedRestClient;
            IKlineSocketClient sharedSocketClient;
            if (symbol.TradingMode == TradingMode.Spot)
            {
                sharedRestClient = restClient.SpotApi.SharedClient;
                sharedSocketClient = socketClient.SpotApi.SharedClient;
            }
            else
            {
                sharedRestClient = restClient.PerpetualFuturesApi.SharedClient;
                sharedSocketClient = socketClient.PerpetualFuturesApi.SharedClient;
            }

            return new KlineTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                sharedSocketClient,
                symbol,
                interval,
                limit,
                period
                );
        }

        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBingXRestClient>() ?? new BingXRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBingXSocketClient>() ?? new BingXSocketClient();

            IRecentTradeRestClient sharedRestClient;
            ITradeSocketClient sharedSocketClient;
            if (symbol.TradingMode == TradingMode.Spot) {
                sharedRestClient = restClient.SpotApi.SharedClient;
                sharedSocketClient = socketClient.SpotApi.SharedClient;
            }
            else {
                sharedRestClient = restClient.PerpetualFuturesApi.SharedClient;
                sharedSocketClient = socketClient.PerpetualFuturesApi.SharedClient;
            }

            return new TradeTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                null,
                sharedSocketClient,
                symbol,
                limit,
                period
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig? config = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBingXRestClient>() ?? new BingXRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBingXSocketClient>() ?? new BingXSocketClient();
            return new BingXUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BingXUserSpotDataTracker>>() ?? new NullLogger<BingXUserSpotDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, ApiCredentials credentials, SpotUserDataTrackerConfig? config = null, BingXEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IBingXUserClientProvider>() ?? new BingXUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new BingXUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BingXUserSpotDataTracker>>() ?? new NullLogger<BingXUserSpotDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker BingXUserPerpetualFuturesDataTracker(FuturesUserDataTrackerConfig? config = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBingXRestClient>() ?? new BingXRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBingXSocketClient>() ?? new BingXSocketClient();
            return new BingXUserPerpetualFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BingXUserPerpetualFuturesDataTracker>>() ?? new NullLogger<BingXUserPerpetualFuturesDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker BingXUserPerpetualFuturesDataTracker(string userIdentifier, ApiCredentials credentials, FuturesUserDataTrackerConfig? config = null, BingXEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IBingXUserClientProvider>() ?? new BingXUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new BingXUserPerpetualFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BingXUserPerpetualFuturesDataTracker>>() ?? new NullLogger<BingXUserPerpetualFuturesDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }
    }
}
