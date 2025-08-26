using System;
using System.Collections.Generic;
using System.Net.Http;
using BingX.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Linq;
using System.Globalization;

namespace BingX.Net
{
    internal class BingXAuthenticationProvider : AuthenticationProvider
    {
        public BingXAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            request.Headers.Add("X-BX-APIKEY", ApiKey);

            if (!request.Authenticated)
                return;

            var timestamp = GetMillisecondTimestampLong(apiClient);
            var parameters = request.GetPositionParameters();
            parameters.Add("timestamp", timestamp);
            if (!parameters.ContainsKey("recvWindow"))
            {
                var receiveWindow = ((BingXRestOptions)apiClient.ClientOptions).ReceiveWindow ?? TimeSpan.FromSeconds(5);
                parameters.Add("recvWindow", (int)receiveWindow.TotalMilliseconds);
            }

            if (request.ParameterPosition == HttpMethodParameterPosition.InBody)
            {
                var bodyString = string.Join("&", parameters.OrderBy(p => p.Key).Select(o => o.Key + "=" + (o.Value is bool ? o.Value?.ToString()!.ToLowerInvariant() : string.Format(CultureInfo.InvariantCulture, "{0}", o.Value))));
                var signature = SignHMACSHA256(bodyString, SignOutputType.Hex);
                parameters.Add("signature", signature);
                if (request.BodyFormat == RequestBodyFormat.FormData)
                    request.SetBodyContent($"{bodyString}&signature={signature}");
            }
            else
            {
                var queryString = parameters.CreateParamString(false, request.ArraySerialization);
                var signature = SignHMACSHA256(queryString, SignOutputType.Hex);
                parameters.Add("signature", signature);
                request.SetQueryString($"{queryString}&signature={signature}");
            }
        }
    }
}
