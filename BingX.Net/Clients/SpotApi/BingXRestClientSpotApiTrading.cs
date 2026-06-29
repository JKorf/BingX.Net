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
using CryptoExchange.Net;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class BingXRestClientSpotApiTrading : IBingXRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal BingXRestClientSpotApiTrading(ILogger logger, BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, decimal? stopPrice = null, string? clientOrderId = null, TimeInForce? timeInForce = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("side", side);
            parameters.Add("type", type);
            parameters.Add("quantity", quantity);
            parameters.Add("price", price);
            parameters.Add("quoteOrderQty", quoteQuantity);
            parameters.Add("stopPrice", stopPrice);
            parameters.Add("newClientOrderId", clientOrderId);
            parameters.Add("timeInForce", timeInForce);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrder>(request, parameters, ct,
                additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrder[]>> PlaceMultipleOrdersAsync(IEnumerable<BingXPlaceOrderRequest> orders, bool? sync = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "data", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(orders) }
            };
            parameters.Add("sync", sync);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderWrapper>(request, parameters, ct,
                additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXOrder[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameter.Add("orderId", orderId);
            parameter.Add("clientOrderID", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/trade/cancel", BingXExchange.RateLimiter.RestAccount2, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrder>(request, parameter, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXCancelsResult>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIds = null, IEnumerable<string>? clientOrderIds = null, bool? processPartialSuccess = null, CancellationToken ct = default)
        {
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameter.Add("process", processPartialSuccess == null ? null : processPartialSuccess == true ? 1 : 0);
            parameter.Add("orderIds", orderIds == null? null: string.Join(",", orderIds));
            parameter.Add("clientOrderIDs", clientOrderIds == null ? null : string.Join(",", clientOrderIds));

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/trade/cancelOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXCancelsResult>(request, parameter, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrder[]>> CancelAllOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/trade/cancelOpenOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderWrapper>(request, parameter, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXOrder[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<HttpResult<BingXCancelAfterResult>> CancelAllOrdersAfterAsync(bool activate, int cancelAfterSeconds, CancellationToken ct = default)
        {
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "type", activate ? "ACTIVATE": "CLOSE" },
                { "timeOut", cancelAfterSeconds }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/trade/cancelAllAfter", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXCancelAfterResult>(request, parameter, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrderDetails>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderID", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/trade/query", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXOrderDetails>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrderDetails[]>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/trade/openOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXOrderDetails[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrderDetails[]>> GetOrdersAsync(string? symbol = null, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("status", status);
            parameters.Add("type", type);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/trade/historyOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXOrderDetails[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<BingXUserTrade[]>> GetUserTradesAsync(string symbol, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("status", status);
            parameters.Add("type", type);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("fromId", fromId);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/trade/myTrades", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXUserTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Trades);
        }

        #endregion

        #region Place Oco Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXOcoOrder[]>> PlaceOcoOrderAsync(string symbol, OrderSide side, decimal quantity, decimal limitPrice, decimal orderPrice, decimal triggerPrice, string? clientOrderId = null, string? aboveClientOrderId = null, string? belowClientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("side", side);
            parameters.Add("quantity", quantity);
            parameters.Add("limitPrice", limitPrice);
            parameters.Add("orderPrice", orderPrice);
            parameters.Add("triggerPrice", triggerPrice);
            parameters.Add("listClientOrderId", clientOrderId);
            parameters.Add("aboveClientOrderId", aboveClientOrderId);
            parameters.Add("belowClientOrderId", belowClientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/oco/order", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Oco Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXOrderId>> CancelOcoOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/spot/v1/oco/cancel", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Oco Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXOcoOrder[]>> GetOcoOrderAsync(string? orderListId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("orderListId", orderListId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/oco/orderList", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Oco Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXOcoOrder[]>> GetOpenOcoOrdersAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/oco/openOrderList", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Oco Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXOcoOrder[]>> GetClosedOcoOrdersAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/spot/v1/oco/historyOrderList", BingXExchange.RateLimiter.RestAccount1, 1, true);
            var result = await _baseClient.SendAsync<BingXOcoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
