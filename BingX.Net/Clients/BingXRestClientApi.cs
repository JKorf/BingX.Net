using BingX.Net.Objects.Internal;
using BingX.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients
{
    /// <summary>
    /// BingX rest client base
    /// </summary>
    internal abstract class BingXRestClientApi : RestApiClient<BingXEnvironment, BingXAuthenticationProvider, BingXCredentials>
    {
        private IStringMessageSerializer? _serializer;

        internal new BingXRestOptions ClientOptions => (BingXRestOptions)base.ClientOptions;

        #region Api clients
        /// <inheritdoc />
        public string ExchangeName => "BingX";
        #endregion

        #region constructor/destructor
        internal BingXRestClientApi(ILogger logger, HttpClient? httpClient, BingXRestOptions options, RestApiOptions apiOptions)
            : base(logger, BingXExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, apiOptions)
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
        protected override BingXAuthenticationProvider CreateAuthenticationProvider(BingXCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            return await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null, Dictionary<string, string>? additionalHeaders = null) where T : class
        {
            var result = await base.SendAsync<BingXResult<T>>(definition, parameters, cancellationToken, additionalHeaders, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            if (result.Data.Code != 0)
                return HttpResult.Fail<T>(result, new ServerError(result.Data.Code.ToString(), GetErrorInfo(result.Data.Code, result.Data.Message)));

            return HttpResult.Ok(result, result.Data.Data!);
        }

        internal async Task<HttpResult<T>> SendRawAsync<T>( RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            return await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, Parameters parameters, string contentType)
        {
            if (contentType == Constants.JsonContentHeader)
            {
                var stringData = (_serializer ??= (IStringMessageSerializer)CreateSerializer()).Serialize(parameters);
                request.SetContent(stringData, Encoding.UTF8, contentType);
            }
            else
            {
                var stringData = parameters.CreateParamString(false, BingXExchange._parameterSerializationSettings.Array);
                request.SetContent(stringData, Encoding.UTF8, contentType);
            }
        }
    }
}
