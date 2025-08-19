using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BingX.Net.Objects.Options;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects.Errors;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc cref="IBingXRestClientPerpetualFuturesApi" />
    internal partial class BingXRestClientPerpetualFuturesApi : BingXRestClientApi, IBingXRestClientPerpetualFuturesApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Perpetual Futures Api");

        protected override ErrorMapping ErrorMapping => BingXErrors.FuturesErrors;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBingXRestClientPerpetualFuturesApiTrading Trading { get; }
        #endregion

        #region constructor/destructor
        internal BingXRestClientPerpetualFuturesApi(ILogger logger, HttpClient? httpClient, BingXRestOptions options)
            : base(logger, httpClient, options, options.FuturesOptions)
        {
            Account = new BingXRestClientPerpetualFuturesApiAccount(this);
            ExchangeData = new BingXRestClientPerpetualFuturesApiExchangeData(logger, this);
            Trading = new BingXRestClientPerpetualFuturesApiTrading(logger, this);

            ArraySerialization = ArrayParametersSerialization.JsonArray;
        }

        #endregion

        public IBingXRestClientPerpetualFuturesApiShared SharedClient => this;

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
