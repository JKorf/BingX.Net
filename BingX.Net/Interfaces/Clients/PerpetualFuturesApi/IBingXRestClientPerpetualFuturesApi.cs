using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace BingX.Net.Interfaces.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// BingX futures API endpoints
    /// </summary>
    public interface IBingXRestClientPerpetualFuturesApi : IRestApiClient<BingXCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IBingXRestClientPerpetualFuturesApiAccount"/>
        public IBingXRestClientPerpetualFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market data
        /// </summary>
        /// <see cref="IBingXRestClientPerpetualFuturesApiExchangeData"/>
        public IBingXRestClientPerpetualFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IBingXRestClientPerpetualFuturesApiTrading"/>
        public IBingXRestClientPerpetualFuturesApiTrading Trading { get; }

        /// <summary>
        /// [V1] Get the shared rest requests client. For new implementations prefer using <see cref="SharedApi"/>
        /// </summary>
        public IBingXRestClientPerpetualFuturesApiShared SharedClient { get; }
        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IBingXRestClientPerpetualFuturesSharedApi SharedApi { get; }

    }
}
