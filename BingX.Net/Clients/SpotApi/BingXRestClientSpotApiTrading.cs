using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using System.Collections.Generic;
using CryptoExchange.Net.Objects;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Data.Common;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BingXRestClientSpotApiTrading : IBingXRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientSpotApi _baseClient;
        private readonly ILogger _logger;
        private readonly string _brokerId;

        internal BingXRestClientSpotApiTrading(ILogger logger, BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;

            _brokerId = !string.IsNullOrEmpty(baseClient.ClientOptions.BrokerId) ? baseClient.ClientOptions.BrokerId! : "easytrading";
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, decimal? stopPrice = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddEnum("side", side);
            parameters.AddEnum("type", type);
            parameters.AddOptional("quantity", quantity);
            parameters.AddOptional("price", price);
            parameters.AddOptional("quoteOrderQty", quoteQuantity);
            parameters.AddOptional("stopPrice", stopPrice);
            parameters.AddOptional("newClientOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXOrder>(request, parameters, ct,
                additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderPlaced(new OrderId { Id = result.Data.OrderId.ToString(), SourceObject = result.Data });
            return result;
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXOrder>>> PlaceMultipleOrdersAsync(IEnumerable<BingXPlaceOrderRequest> orders, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "data", new SystemTextJsonMessageSerializer().Serialize(orders) }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true, 2, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXOrderWrapper>(request, parameters, ct,
                additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<IEnumerable<BingXOrder>>(result.Data?.Orders);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameter.AddOptional("orderId", orderId);
            parameter.AddOptional("clientOrderID", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancel", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXOrder>(request, parameter, ct).ConfigureAwait(false);            
            if (result)
                _baseClient.InvokeOrderCanceled(new OrderId { Id = result.Data.OrderId.ToString(), SourceObject = result.Data });
            return result;
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXOrder>>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIds = null, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameter.AddOptional("orderIds", orderIds == null? null: string.Join(",", orderIds));
            parameter.AddOptional("clientOrderIDs", clientOrderIds == null ? null : string.Join(",", clientOrderIds));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancelOrders", BingXExchange.RateLimiter.RestAccount2, 1, true, 2, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXOrderWrapper>(request, parameter, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXOrder>>(result.Data?.Orders);
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXOrder>>> CancelAllOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancelOpenOrders", BingXExchange.RateLimiter.RestAccount2, 1, true, 2, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXOrderWrapper>(request, parameter, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXOrder>>(result.Data?.Orders);
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<WebCallResult<BingXCancelAfterResult>> CancelAllOrdersAfterAsync(bool activate, int cancelAfterSeconds, CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "type", activate ? "ACTIVATE": "CLOSE" },
                { "timeOut", cancelAfterSeconds }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancelAllAfter", BingXExchange.RateLimiter.RestAccount1, 1, true, 2, TimeSpan.FromSeconds(1));
            return await _baseClient.SendAsync<BingXCancelAfterResult>(request, parameter, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrderDetails>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderID", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/query", BingXExchange.RateLimiter.RestAccount1, 1, true, 10, TimeSpan.FromSeconds(1));
            return await _baseClient.SendAsync<BingXOrderDetails>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXOrderDetails>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/openOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, 10, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXOrderDetails>>(result.Data?.Orders);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXOrderDetails>>> GetOrdersAsync(string? symbol = null, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptionalEnum("status", status);
            parameters.AddOptionalEnum("type", type);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("pageIndex", page);
            parameters.AddOptional("pageSize", pageSize);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/historyOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, 10, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXOrderDetails>>(result.Data?.Orders);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptionalEnum("status", status);
            parameters.AddOptionalEnum("type", type);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("fromId", fromId);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/myTrades", BingXExchange.RateLimiter.RestAccount1, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXUserTrade>>(result.Data?.Trades);
        }

        #endregion
    }
}
