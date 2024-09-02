using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis.Interfaces;
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
        public IBingXRestClientPerpetualFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market data
        /// </summary>
        public IBingXRestClientPerpetualFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        public IBingXRestClientPerpetualFuturesApiTrading Trading { get; }

        public IBingXRestClientPerpetualFuturesApiShared SharedClient { get; }

    }
}
