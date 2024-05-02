using System;
using System.Collections.Generic;
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

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, IDictionary<string, object> uriParams, IDictionary<string, object> bodyParams, Dictionary<string, string> headers, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, RequestBodyFormat bodyFormat)
        {
            headers.Add("X-BX-APIKEY", GetApiKey());

            if (!auth)
                return;

            var parameters = parameterPosition == HttpMethodParameterPosition.InUri ? uriParams : bodyParams;
            var timestamp = DateTimeConverter.ConvertToMilliseconds(GetTimestamp(apiClient)).Value;
            parameters.Add("timestamp", timestamp);
            if (!parameters.ContainsKey("recvWindow"))
            {
                var receiveWindow = ((BingXRestOptions)apiClient.ClientOptions).ReceiveWindow ?? TimeSpan.FromSeconds(5);
                parameters.Add("recvWindow", (int)receiveWindow.TotalMilliseconds);
            }

            var parameterSignData = parameters.CreateParamString(true, arraySerialization);
            parameters.Add("signature", SignHMACSHA256(parameterSignData, SignOutputType.Hex));
        }
    }
}
