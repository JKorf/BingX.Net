using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BingX.Net.Objects.Options;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Options;

namespace BingX.Net.Clients
{
    /// <summary>
    /// BingX rest client base
    /// </summary>
    public abstract class BingXRestClientApi : RestApiClient
    {
        private IStringMessageSerializer? _serializer;

        internal new BingXRestOptions ClientOptions => (BingXRestOptions)base.ClientOptions;

        #region Api clients
        /// <inheritdoc />
        public string ExchangeName => "BingX";
        #endregion

        #region constructor/destructor
        internal BingXRestClientApi(ILogger logger, HttpClient? httpClient, BingXRestOptions options, RestApiOptions apiOptions)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, apiOptions)
        {
            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InUri;
            RequestBodyFormat = RequestBodyFormat.FormData;
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => BingXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext));
        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(SerializerOptions.WithConverters(BingXExchange._serializerContext));

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            return await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null, Dictionary<string, string>? additionalHeaders = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight, additionalHeaders);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null, Dictionary<string, string>? additionalHeaders = null) where T : class
        {
            var result = await base.SendAsync<BingXResult<T>>(baseAddress, definition, parameters, cancellationToken, additionalHeaders, weight).ConfigureAwait(false);
            if (!result.Success)
                return result.As<T>(null);

            if (result.Data.Code != 0)
                return result.AsError<T>(new ServerError(result.Data.Code.ToString(), GetErrorInfo(result.Data.Code, result.Data.Message)));

            return result.As<T>(result.Data.Data);
        }

        internal Task<WebCallResult<T>> SendRawAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendRawToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendRawToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            return await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Error? TryParseError(RequestDefinition request, KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor)
        {
            var code = accessor.GetValue<int>(MessagePath.Get().Property("code"));
            if (code == 0)
                return null;

            var msg = accessor.GetValue<string>(MessagePath.Get().Property("msg"));
            return new ServerError(code.ToString(), GetErrorInfo(code, msg));
        }

        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, IDictionary<string, object> parameters, string contentType)
        {
            if (contentType == Constants.JsonContentHeader)
            {
                var stringData = (_serializer ??= (IStringMessageSerializer)CreateSerializer()).Serialize(parameters);
                request.SetContent(stringData, contentType);
            }
            else
            {
                var stringData = parameters.CreateParamString(false, ArraySerialization);
                request.SetContent(stringData, contentType);
            }
        }

        /// <inheritdoc />
        public string GetSymbolName(string baseAsset, string quoteAsset) => baseAsset + "-" + quoteAsset;
    }
}
