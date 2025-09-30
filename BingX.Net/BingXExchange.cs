using BingX.Net.Converters;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net
{
    /// <summary>
    /// BingX exchange information and configuration
    /// </summary>
    public static class BingXExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "BingX";

        /// <summary>
        /// Exchange name
        /// </summary>
        public static string DisplayName => "BingX";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "https://raw.githubusercontent.com/JKorf/BingX.Net/master/BingX.Net/Icon/BingX.png";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.bingx.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://bingx-api.github.io/docs"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        internal static JsonSerializerContext _serializerContext = JsonSerializerContextCache.GetOrCreate<BingXSourceGenerationContext>();

        /// <summary>
        /// Format a base and quote asset to a BingX recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            return baseAsset.ToUpperInvariant() + "-" + quoteAsset.ToUpperInvariant();
        }

        /// <summary>
        /// Rate limiter configuration for the BingX API
        /// </summary>
        public static BingXRateLimiters RateLimiter { get; } = new BingXRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the BingX API
    /// </summary>
    public class BingXRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;

        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal BingXRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            RestMarket = new RateLimitGate("Rest Market")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding)) // As suggested by BingX API support: IP limit of 10 requests per 1 second in total 
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)) // IP limit of 100 requests per 10 seconds in total 
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 1000, TimeSpan.FromSeconds(60), RateLimitWindowType.Sliding)); // IP limit of 500 requests per 60 seconds in total 
            RestAccount1 = new RateLimitGate("Rest Account 1")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 2000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)) // IP limit of 1000 requests per 10 seconds in total 
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerEndpoint, new List<IGuardFilter>(), 300, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // IP limit of 100 requests per 10 seconds per endpoint
            RestAccount2 = new RateLimitGate("Rest Account 2")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 2000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)) // IP limit of 1000 requests per 10 seconds in total 
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerEndpoint, new List<IGuardFilter>(), 300, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // IP limit of 200 requests per 10 seconds per endpoint

            RestMarket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestMarket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            RestAccount1.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestAccount1.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            RestAccount2.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestAccount2.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate RestMarket { get; private set; }
        internal IRateLimitGate RestAccount1 { get; private set; }
        internal IRateLimitGate RestAccount2 { get; private set; }

    }
}
