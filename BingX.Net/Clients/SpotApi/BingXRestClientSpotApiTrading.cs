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
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.RateLimiting.Guards;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class BingXRestClientSpotApiTrading : IBingXRestClientSpotApiTrading
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
        public async Task<WebCallResult<BingXOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, decimal? stopPrice = null, string? clientOrderId = null, TimeInForce? timeInForce = null, CancellationToken ct = default)
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
            parameters.AddOptionalEnum("timeInForce", timeInForce);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrder>(request, parameters, ct,
                additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrder[]>> PlaceMultipleOrdersAsync(IEnumerable<BingXPlaceOrderRequest> orders, bool? sync = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "data", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(orders) }
            };
            parameters.AddOptional("sync", sync);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderWrapper>(request, parameters, ct,
                additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<BingXOrder[]>(result.Data?.Orders);
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancel", BingXExchange.RateLimiter.RestAccount2, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrder>(request, parameter, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXCancelsResult>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIds = null, IEnumerable<string>? clientOrderIds = null, bool? processPartialSuccess = null, CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameter.AddOptional("process", processPartialSuccess == null ? null : processPartialSuccess == true ? 1 : 0);
            parameter.AddOptional("orderIds", orderIds == null? null: string.Join(",", orderIds));
            parameter.AddOptional("clientOrderIDs", clientOrderIds == null ? null : string.Join(",", clientOrderIds));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancelOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXCancelsResult>(request, parameter, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrder[]>> CancelAllOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancelOpenOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderWrapper>(request, parameter, ct).ConfigureAwait(false);
            return result.As<BingXOrder[]>(result.Data?.Orders);
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

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/trade/cancelAllAfter", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/query", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXOrderDetails>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrderDetails[]>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/openOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXOrderDetails[]>(result.Data?.Orders);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrderDetails[]>> GetOrdersAsync(string? symbol = null, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/historyOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXOrderDetails[]>(result.Data?.Orders);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<BingXUserTrade[]>> GetUserTradesAsync(string symbol, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, long? fromId = null, int? limit = null, CancellationToken ct = default)
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/trade/myTrades", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXUserTrade[]>(result.Data?.Trades);
        }

        #endregion

        #region Place Oco Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOcoOrder[]>> PlaceOcoOrderAsync(string symbol, OrderSide side, decimal quantity, decimal limitPrice, decimal orderPrice, decimal triggerPrice, string? clientOrderId = null, string? aboveClientOrderId = null, string? belowClientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddEnum("side", side);
            parameters.Add("quantity", quantity);
            parameters.Add("limitPrice", limitPrice);
            parameters.Add("orderPrice", orderPrice);
            parameters.Add("triggerPrice", triggerPrice);
            parameters.AddOptional("listClientOrderId", clientOrderId);
            parameters.AddOptional("aboveClientOrderId", aboveClientOrderId);
            parameters.AddOptional("belowClientOrderId", belowClientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/oco/order", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Oco Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOrderId>> CancelOcoOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/spot/v1/oco/cancel", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Oco Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOcoOrder[]>> GetOcoOrderAsync(string? orderListId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderListId", orderListId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/oco/orderList", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Oco Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOcoOrder[]>> GetOpenOcoOrdersAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/oco/openOrderList", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Oco Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXOcoOrder[]>> GetClosedOcoOrdersAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/oco/historyOrderList", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
