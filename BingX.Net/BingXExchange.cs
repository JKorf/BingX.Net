using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using System;
using System.Collections.Generic;

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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal BingXRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            RestMarket = new RateLimitGate("Spot Rest Market")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 100, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // IP limit of 100 requests per 10 seconds in total 
            RestAccount1 = new RateLimitGate("Spot Rest Account 1")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)) // IP limit of 1000 requests per 10 seconds in total 
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerEndpoint, new List<IGuardFilter>(), 100, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // IP limit of 100 requests per 10 seconds per endpoint
            RestAccount2 = new RateLimitGate("Spot Rest Account 1")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new List<IGuardFilter>(), 1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)) // IP limit of 1000 requests per 10 seconds in total 
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerEndpoint, new List<IGuardFilter>(), 200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // IP limit of 200 requests per 10 seconds per endpoint

            RestMarket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestAccount1.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestAccount2.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
        }


        internal IRateLimitGate RestMarket { get; private set; }
        internal IRateLimitGate RestAccount1 { get; private set; }
        internal IRateLimitGate RestAccount2 { get; private set; }

    }
}
