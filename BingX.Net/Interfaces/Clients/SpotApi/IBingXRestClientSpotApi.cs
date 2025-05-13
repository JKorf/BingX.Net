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
        /// <see cref="IBingXRestClientSpotApiAccount"/>
        public IBingXRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IBingXRestClientSpotApiExchangeData"/>
        public IBingXRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IBingXRestClientSpotApiTrading"/>
        public IBingXRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBingXRestClientSpotApiShared SharedClient { get; }

    }
}
