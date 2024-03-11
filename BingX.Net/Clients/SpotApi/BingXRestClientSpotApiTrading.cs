using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using System.Collections.Generic;
using CryptoExchange.Net.Objects;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BingXRestClientSpotApiTrading : IBingXRestClientSpotApiTrading
    {
        private readonly BingXRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal BingXRestClientSpotApiTrading(ILogger logger, BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, decimal? stopPrice = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameter.AddEnum("side", side);
            parameter.AddEnum("type", type);
            parameter.AddOptional("quantity", quantity);
            parameter.AddOptional("price", price);
            parameter.AddOptional("quoteOrderQty", quoteQuantity);
            parameter.AddOptional("stopPrice", stopPrice);
            parameter.AddOptional("newClientOrderId", clientOrderId);
            return await _baseClient.SendRequestInternal<BingXOrder>(_baseClient.GetUri("/openApi/spot/v1/trade/order"), HttpMethod.Post, ct, parameter, true).ConfigureAwait(false);
        }

        #endregion
    }
}
