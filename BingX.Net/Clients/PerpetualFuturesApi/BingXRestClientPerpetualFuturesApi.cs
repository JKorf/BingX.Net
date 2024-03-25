using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.MessageParsing;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc cref="IBingXRestClientPerpetualFuturesApi" />
    public class BingXRestClientPerpetualFuturesApi : RestApiClient, IBingXRestClientPerpetualFuturesApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Perpetual Futures Api");
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "BingX";
        #endregion

        internal new BingXRestOptions ClientOptions => (BingXRestOptions)base.ClientOptions;

        #region constructor/destructor
        internal BingXRestClientPerpetualFuturesApi(ILogger logger, HttpClient? httpClient, BingXRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress!, options, options.FuturesOptions)
        {
            Account = new BingXRestClientPerpetualFuturesApiAccount(this);
            ExchangeData = new BingXRestClientPerpetualFuturesApiExchangeData(logger, this);
            Trading = new BingXRestClientPerpetualFuturesApiTrading(logger, this);

            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InUri;
            RequestBodyFormat = RequestBodyFormat.FormData;
            ArraySerialization = ArrayParametersSerialization.JsonArray;
        }

        #endregion


        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();
        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BingXAuthenticationProvider(credentials);

        internal Uri GetUri(string path) => new Uri(BaseAddress.AppendPath(path));

        internal async Task<WebCallResult> SendRequestInternal(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false)
        {
            return await SendRequestAsync(uri, method, cancellationToken, parameters, signed, null, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
        }

        internal async Task<WebCallResult<T>> SendRequestInternal<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false, Dictionary<string, string>? additionalHeaders = null) where T : class
        {
            var result = await SendRequestAsync<BingXResult<T>>(uri, method, cancellationToken, parameters, signed, null, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit, additionalHeaders: additionalHeaders).ConfigureAwait(false);
            if (!result.Success)
                return result.As<T>(null);

            if (result.Data.Code != 0)
                return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message!));

            return result.As<T>(result.Data.Data);
        }

        internal async Task<WebCallResult<T>> SendRequestInternalRaw<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        {
            return await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, null, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override ServerError? TryParseError(IMessageAccessor accessor)
        {
            var code = accessor.GetValue<int>(MessagePath.Get().Property("code"));
            if (code == 0)
                return null;

            var msg = accessor.GetValue<string>(MessagePath.Get().Property("msg"));
            return new ServerError(code, msg!);
        }

        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, SortedDictionary<string, object> parameters, string contentType)
        {
            var stringData = parameters.CreateParamString(false, ArraySerialization);
            request.SetContent(stringData, contentType);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp, ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;
    }
}
