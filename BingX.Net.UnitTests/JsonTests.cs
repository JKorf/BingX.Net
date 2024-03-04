using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Authentication;
using BingX.Net.Interfaces.Clients;
using BingX.Net.UnitTests.TestImplementations;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class JsonTests
    {
        private JsonToObjectComparer<IBingXRestClient> _comparer = new JsonToObjectComparer<IBingXRestClient>((json) => TestHelpers.CreateResponseClient(json, options =>
        {
            options.ApiCredentials = new ApiCredentials("123", "123");
            options.SpotOptions.RateLimiters = new List<IRateLimiter>();
            options.SpotOptions.AutoTimestamp = false;

            options.FuturesOptions.RateLimiters = new List<IRateLimiter>();
            options.FuturesOptions.AutoTimestamp = false;
        }));

        [Test]
        public async Task ValidateSpotAccountCalls()
        {
            await _comparer.ProcessSubject(
                "Spot/Account",
                c => c.SpotApi.Account);
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            await _comparer.ProcessSubject(
                "Spot/ExchangeData",
                c => c.SpotApi.ExchangeData, useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "GetSymbolsAsync", "data:symbols" },
                    { "GetRecentTradesAsync", "data" },
                });
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            await _comparer.ProcessSubject(
                "Spot/Trading",
                c => c.SpotApi.Trading);
        }

        [Test]
        public async Task ValidateFuturesAccountCalls()
        {
            await _comparer.ProcessSubject(
                "Futures/Account",
                c => c.FuturesApi.Account);
        }

        [Test]
        public async Task ValidateFuturesExchangeDataCalls()
        {
            await _comparer.ProcessSubject(
                "Futures/ExchangeData",
                c => c.FuturesApi.ExchangeData);
        }

        [Test]
        public async Task ValidateFuturesTradingCalls()
        {
            await _comparer.ProcessSubject(
                "Futures/Trading",
                c => c.FuturesApi.Trading);
        }
    }
}
