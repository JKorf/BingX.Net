using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Authentication;
using BingX.Net.Interfaces.Clients;
using BingX.Net.UnitTests.TestImplementations;
using BingX.Net.Clients;
using CryptoExchange.Test.Net;
using CryptoExchange.Net.Objects;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class EndpointTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
            });
            var tester = new EndpointTester<BingXRestClient>(client, "Endpoints/Spot/ExchangeData", "https://open-api.bingx.com", IsAuthenticated, "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetBookPriceAsync("ETHUSDT"), "GetBookPrice", userSingleArrayItem: true);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT"), "GetTradeHistory");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync("ETHUSDT"), "GetTickers");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl.Contains("signature");
        }
    }
}
