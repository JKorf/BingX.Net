using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using BingX.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace BingX.Net
{
    internal class BingXAuthenticationProvider : AuthenticationProvider
    {
        public string GetApiKey() => _credentials.Key!.GetString();

        public BingXAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, RequestBodyFormat bodyFormat, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>() { };

            if (!auth)
                return;

            headers.Add("X-BX-APIKEY", GetApiKey());
            var parameters = parameterPosition == HttpMethodParameterPosition.InUri ? uriParameters : bodyParameters;
            var timestamp = DateTimeConverter.ConvertToMilliseconds(GetTimestamp(apiClient)).Value;
            parameters.Add("timestamp", timestamp);
            var receiveWindow = ((BingXRestOptions)apiClient.ClientOptions).ReceiveWindow ?? TimeSpan.FromSeconds(5);
            parameters.Add("recvWindow", (int)receiveWindow.TotalMilliseconds);

            uri = uri.SetParameters(uriParameters, arraySerialization);
            var signData = parameterPosition == HttpMethodParameterPosition.InUri ? uri.Query.Replace("?", "") : string.Join("&", parameters.Select(p => p.Key + "=" + string.Format(CultureInfo.InvariantCulture, "{0}", p.Value)));
            parameters.Add("signature", SignHMACSHA256(signData, SignOutputType.Hex));
        }
    }
}
