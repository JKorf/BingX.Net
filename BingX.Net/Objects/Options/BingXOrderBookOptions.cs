using CryptoExchange.Net.Objects.Options;
using System;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// Options for the BingX SymbolOrderBook
    /// </summary>
    public class BingXOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static BingXOrderBookOptions Default { get; set; } = new BingXOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal BingXOrderBookOptions Copy()
        {
            var result = Copy<BingXOrderBookOptions>();
            result.Limit = Limit;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
