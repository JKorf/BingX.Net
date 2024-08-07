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

        public override void AuthenticateRequest(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            ref IDictionary<string, object>? uriParameters,
            ref IDictionary<string, object>? bodyParameters,
            ref Dictionary<string, string>? headers,
            bool auth,
            ArrayParametersSerialization arraySerialization,
            HttpMethodParameterPosition parameterPosition,
            RequestBodyFormat requestBodyFormat)
        {
            headers ??= new Dictionary<string, string>();
            headers.Add("X-BX-APIKEY", ApiKey);

            if (!auth)
                return;

            IDictionary<string, object> parameters;
            if (parameterPosition == HttpMethodParameterPosition.InUri)
            {
                uriParameters ??= new Dictionary<string, object>();
                parameters = uriParameters;
            }
            else
            {
                bodyParameters ??= new Dictionary<string, object>();
                parameters = bodyParameters;
            }
            var timestamp = DateTimeConverter.ConvertToMilliseconds(GetTimestamp(apiClient)).Value;
            parameters.Add("timestamp", timestamp);
            if (!parameters.ContainsKey("recvWindow"))
            {
                var receiveWindow = ((BingXRestOptions)apiClient.ClientOptions).ReceiveWindow ?? TimeSpan.FromSeconds(5);
                parameters.Add("recvWindow", (int)receiveWindow.TotalMilliseconds);
            }

            string parameterSignData;
            if (parameterPosition == HttpMethodParameterPosition.InBody)
                parameterSignData = string.Join("&", parameters.OrderBy(p => p.Key).Select(o => o.Key + "=" + string.Format(CultureInfo.InvariantCulture, "{0}", o.Value)));
            else
                parameterSignData = parameters.CreateParamString(true, arraySerialization);

            parameters.Add("signature", SignHMACSHA256(parameterSignData, SignOutputType.Hex));
        }
    }
}
