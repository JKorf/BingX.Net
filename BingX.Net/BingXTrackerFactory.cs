using BingX.Net.Interfaces;
using BingX.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace BingX.Net
{
    /// <inheritdoc />
    public class BingXTrackerFactory : IBingXTrackerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BingXTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            IKlineRestClient restClient;
            IKlineSocketClient socketClient;
            if (symbol.TradingMode == TradingMode.Spot)
            {
                restClient = _serviceProvider.GetRequiredService<IBingXRestClient>().SpotApi.SharedClient;
                socketClient = _serviceProvider.GetRequiredService<IBingXSocketClient>().SpotApi.SharedClient;
            }
            else
            {
                restClient = _serviceProvider.GetRequiredService<IBingXRestClient>().PerpetualFuturesApi.SharedClient;
                socketClient = _serviceProvider.GetRequiredService<IBingXSocketClient>().PerpetualFuturesApi.SharedClient;
            }

            return new KlineTracker(
                _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                restClient,
                socketClient,
                symbol,
                interval,
                limit,
                period
                );
        }

        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            IRecentTradeRestClient restClient;
            ITradeSocketClient socketClient;
            if (symbol.TradingMode == TradingMode.Spot) {
                restClient = _serviceProvider.GetRequiredService<IBingXRestClient>().SpotApi.SharedClient;
                socketClient = _serviceProvider.GetRequiredService<IBingXSocketClient>().SpotApi.SharedClient;
            }
            else {
                restClient = _serviceProvider.GetRequiredService<IBingXRestClient>().PerpetualFuturesApi.SharedClient;
                socketClient = _serviceProvider.GetRequiredService<IBingXSocketClient>().PerpetualFuturesApi.SharedClient;
            }

            return new TradeTracker(
                _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                restClient,
                socketClient,
                symbol,
                limit,
                period
                );
        }
    }
}
