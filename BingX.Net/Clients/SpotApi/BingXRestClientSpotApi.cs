using BingX.Net.Clients.MessageHandlers;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBingXRestClientSpotApi" />
    internal partial class BingXRestClientSpotApi : BingXRestClientApi, IBingXRestClientSpotApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => BingXErrors.SpotErrors;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBingXRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IBingXRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBingXRestClientSpotApiTrading Trading { get; }
        #endregion

        #region constructor/destructor
        internal BingXRestClientSpotApi(ILogger logger, HttpClient? httpClient, BingXRestOptions options)
            : base(logger, httpClient, options, options.SpotOptions)
        {
            Account = new BingXRestClientSpotApiAccount(this);
            ExchangeData = new BingXRestClientSpotApiExchangeData(logger, this);
            Trading = new BingXRestClientSpotApiTrading(logger, this);
        }
        #endregion

        protected override IRestMessageHandler MessageHandler => new BingXRestMessageHandler(BingXErrors.SpotErrors);

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public IBingXRestClientSpotApiShared SharedClient => this;

    }
}
