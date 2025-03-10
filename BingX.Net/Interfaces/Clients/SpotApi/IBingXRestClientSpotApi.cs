using CryptoExchange.Net.Interfaces;
using System;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX Spot API endpoints
    /// </summary>
    public interface IBingXRestClientSpotApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        public IBingXRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        public IBingXRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        public IBingXRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBingXRestClientSpotApiShared SharedClient { get; }

    }
}
