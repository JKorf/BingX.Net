using NUnit.Framework;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using BingX.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;

namespace BingX.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            var client = new BingXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BingXRestClient>(client, "Endpoints/Spot/ExchangeData", "https://open-api.bingx.com", IsAuthenticated, "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetBookPriceAsync("ETHUSDT"), "GetBookPrice", useSingleArrayItem: true);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT"), "GetTradeHistory");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync("ETHUSDT"), "GetTickers");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl.Contains("signature");
        }
    }
}
