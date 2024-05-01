using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using BingX.Net.Clients;
using NUnit.Framework.Legacy;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using System.Net.Http;
using System.Collections.Generic;
using CryptoExchange.Net.Converters.JsonNet;

namespace BingX.Net.UnitTests
{
    [TestFixture()]
    public class BingXRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new BingXAuthenticationProvider(new ApiCredentials("hO6oQotzTE0S5FRYze2Jx2wGx7eVnJGMolpA1nZyehsoMgCcgKNWQHd4QgTFZuwl4Zt4xMe2PqGBegWXO4A", "mheO6dR8ovSsxZQCOYEFCtelpuxcWGTfHw7te326y6jOwq5WpvFQ9JNljoTwBXZGv5It07m9RXSPpDQEK2w"));
            var client = (RestApiClient)new BingXRestClient().SpotApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "",
                (uriParams, bodyParams, headers) =>
                {
                    return bodyParams["signature"].ToString();
                },
                "8D0D3EA9B592BE3678C33332AB13E9102E093E67255921E15A581146C87C272F",
                new Dictionary<string, object>
                {
                    { "recvWindow", 0 },
                    { "subAccountString", "abc12345" },
                },
                DateTimeConverter.ParseFromLong(1696751141337),
                true,
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<BingXRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<BingXSocketClient>();
        }
    }
}
