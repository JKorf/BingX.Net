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

        protected override ErrorCollection ErrorMapping { get; } = new ErrorCollection(
            [
                new ErrorInfo(ErrorType.SignatureInvalid, false, "Signature error", "100001"),
                new ErrorInfo(ErrorType.SignatureInvalid, false, "Invalid API key", "100413"),

                new ErrorInfo(ErrorType.Unauthorized, false, "Insufficient permissions", "100004"),
                new ErrorInfo(ErrorType.Unauthorized, false, "IP not whitelisted", "100419"),

                new ErrorInfo(ErrorType.TimestampInvalid, false, "Invalid timestamp", "100421"),

                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", "80014"),
                new ErrorInfo(ErrorType.InvalidParameter, false, "Maximum leverage exceeded", "101414"),
                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid or missing arguments", "100400"),

                new ErrorInfo(ErrorType.PriceInvalid, false, "Order price should be higher than estimated liquidation price", "101460"),
                new ErrorInfo(ErrorType.PriceInvalid, false, "Order price not within range", "101211"),

                new ErrorInfo(ErrorType.OrderConfigurationRejected, false, "PostOnly order could not be placed", "101215"),

                new ErrorInfo(ErrorType.InvalidOperation, false, "Order already filled", "80018"),

                new ErrorInfo(ErrorType.UnknownOrder, false, "Order does not exists", "80016"),

                new ErrorInfo(ErrorType.NoPosition, false, "No open position", "80017"),

                new ErrorInfo(ErrorType.BalanceInsufficient, false, "Insufficient margin", "101204"),

                new ErrorInfo(ErrorType.OrderRateLimited, false, "Entrust order limit reached", "80013"),

                new ErrorInfo(ErrorType.RequestRateLimited, false, "Rate limit reached", "100410"),

                new ErrorInfo(ErrorType.SymbolNotTrading, false, "Symbol is not currently trading", "101415"),
            ]
        );
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
