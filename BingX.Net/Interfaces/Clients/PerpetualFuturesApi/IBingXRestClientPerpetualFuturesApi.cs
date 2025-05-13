using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Interfaces;
using System;

namespace BingX.Net.Interfaces.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// BingX futures API endpoints
    /// </summary>
    public interface IBingXRestClientPerpetualFuturesApi : IRestApiClient, IDisposable
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
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBingXRestClientPerpetualFuturesApiShared SharedClient { get; }

    }
}
