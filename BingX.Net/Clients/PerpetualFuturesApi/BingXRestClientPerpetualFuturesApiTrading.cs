using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using System.Threading;
using System.Net.Http;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Linq;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    internal class BingXRestClientPerpetualFuturesApiTrading : IBingXRestClientPerpetualFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientPerpetualFuturesApi _baseClient;

        internal BingXRestClientPerpetualFuturesApiTrading(ILogger logger, BingXRestClientPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Positions

        /// <inheritdoc />
        public async Task<HttpResult<BingXPosition[]>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/user/positions", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXPosition[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Place Test Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrder>> PlaceTestOrderAsync(
            string symbol,
            OrderSide side,
            FuturesOrderType type,
            PositionSide positionSide,
            decimal? quantity = null,
            decimal? price = null,
            bool? reduceOnly = null,
            decimal? stopPrice = null,
            decimal? priceRate = null,

            TakeProfitStopLossMode? stopLossType = null,
            decimal? stopLossStopPrice = null,
            decimal? stopLossPrice = null,
            TriggerType? stopLossTriggerType = null,
            bool? stopLossStopGuaranteed = null,

            TakeProfitStopLossMode? takeProfitType = null,
            decimal? takeProfitStopPrice = null,
            decimal? takeProfitPrice = null,
            TriggerType? takeProfitTriggerType = null,
            bool? takeProfitStopGuaranteed = null,

            TimeInForce? timeInForce = null,
            bool? closePosition = null,
            decimal? triggerPrice = null,
            bool? stopGuaranteed = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameter.Add("side", side);
            parameter.Add("type", type);
            parameter.Add("positionSide", positionSide);
            parameter.Add("reduceOnly", reduceOnly?.ToString().ToLowerInvariant());
            parameter.Add("quantity", quantity);
            parameter.Add("price", price);
            parameter.Add("stopPrice", stopPrice);
            parameter.Add("newClientOrderId", clientOrderId);
            parameter.Add("priceRate", priceRate);
            parameter.Add("timeInForce", timeInForce);
            parameter.Add("closePosition", closePosition?.ToString().ToLowerInvariant());
            parameter.Add("activationPrice", triggerPrice);
            parameter.Add("stopGuaranteed", stopGuaranteed);

            if (stopLossType != null)
            {
                var stopLossParams = new Parameters(BingXExchange._parameterSerializationSettings);
                stopLossParams.Add("type", stopLossType.Value);
                stopLossParams.Add("stopPrice", stopLossStopPrice);
                stopLossParams.Add("price", stopLossPrice);
                stopLossParams.Add("workingType", stopLossTriggerType);
                stopLossParams.Add("stopGuaranteed", stopLossStopGuaranteed?.ToString().ToLowerInvariant());
                parameter.Add("stopLoss", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(stopLossParams));
            }

            if (takeProfitType != null)
            {
                var takeProfitParams = new Parameters(BingXExchange._parameterSerializationSettings);
                takeProfitParams.Add("type", takeProfitType.Value);
                takeProfitParams.Add("stopPrice", takeProfitStopPrice);
                takeProfitParams.Add("price", takeProfitPrice);
                takeProfitParams.Add("workingType", takeProfitTriggerType);
                takeProfitParams.Add("stopGuaranteed", takeProfitStopGuaranteed?.ToString().ToLowerInvariant());
                parameter.Add("takeProfit", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(takeProfitParams));
            }

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/order/test", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrderWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrder>(result);

            return HttpResult.Ok(result, result.Data.Order);
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            FuturesOrderType type,
            PositionSide positionSide,
            decimal? quantity = null,
            decimal? price = null,
            bool? reduceOnly = null,
            decimal? stopPrice = null,
            decimal? priceRate = null,

            TakeProfitStopLossMode? stopLossType = null,
            decimal? stopLossStopPrice = null,
            decimal? stopLossPrice = null,
            TriggerType? stopLossTriggerType = null,
            bool? stopLossStopGuaranteed = null,

            TakeProfitStopLossMode? takeProfitType = null,
            decimal? takeProfitStopPrice = null,
            decimal? takeProfitPrice = null,
            TriggerType? takeProfitTriggerType = null,
            bool? takeProfitStopGuaranteed = null,

            TimeInForce? timeInForce = null,
            bool? closePosition = null,
            decimal? triggerPrice = null,
            bool? stopGuaranteed = null,
            string? clientOrderId = null,
            TriggerType? workingType = null,
            decimal? quoteQuantity = null,
            CancellationToken ct = default)
        {
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameter.Add("side", side);
            parameter.Add("type", type);
            parameter.Add("positionSide", positionSide);
            parameter.Add("reduceOnly", reduceOnly);
            parameter.Add("quantity", quantity);
            parameter.Add("quoteOrderQty", quoteQuantity);
            parameter.Add("price", price);
            parameter.Add("stopPrice", stopPrice);
            parameter.Add("clientOrderId", clientOrderId);
            parameter.Add("priceRate", priceRate);
            parameter.Add("timeInForce", timeInForce);
            parameter.Add("closePosition", closePosition?.ToString().ToLowerInvariant());
            parameter.Add("activationPrice", triggerPrice);
            parameter.Add("stopGuaranteed", stopGuaranteed);
            parameter.Add("workingType", workingType);

            if (stopLossType != null)
            {
                var stopLossParams = new Parameters(BingXExchange._parameterSerializationSettings);
                stopLossParams.Add("type", stopLossType.Value);
                stopLossParams.Add("stopPrice", stopLossStopPrice);
                stopLossParams.Add("price", stopLossPrice);
                stopLossParams.Add("workingType", stopLossTriggerType);
                stopLossParams.Add("stopGuaranteed", stopLossStopGuaranteed?.ToString().ToLowerInvariant());
                var json = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(stopLossParams);
                json = json.Replace("\u0022", "\"");
                parameter.Add("stopLoss", json);
            }

            if (takeProfitType != null)
            {
                var takeProfitParams = new Parameters(BingXExchange._parameterSerializationSettings);
                takeProfitParams.Add("type", takeProfitType.Value);
                takeProfitParams.Add("stopPrice", takeProfitStopPrice);
                takeProfitParams.Add("price", takeProfitPrice);
                takeProfitParams.Add("workingType", takeProfitTriggerType);
                takeProfitParams.Add("stopGuaranteed", takeProfitStopGuaranteed?.ToString().ToLowerInvariant());
                var json = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(takeProfitParams);
                json = json.Replace("\u0022", "\"");
                parameter.Add("takeProfit", json);
            }

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey), requestBodyFormat: RequestBodyFormat.Json);
            var result = await _baseClient.SendAsync<BingXFuturesOrderWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrder>(result);

            return HttpResult.Ok(result, result.Data.Order);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrder[]>> PlaceMultipleOrderAsync(
            IEnumerable<BingXFuturesPlaceOrderRequest> orders,
            bool? sync = null,
            CancellationToken ct = default)
        {
            foreach(var order in orders)
            {
                if (order.StopLoss != null)
                    order.StopLossStr = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(order.StopLoss);

                if (order.TakeProfit != null)
                    order.TakeProfitStr = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(order.TakeProfit);
            }
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "batchOrders", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(orders) }
            };
            parameter.Add("sync", sync);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrder[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXEditResult>> EditOrderAsync(
            long? orderId,
            string? clientOrderId,
            string symbol,
            decimal quantity,
            CancellationToken ct = default)
        {
            var parameter = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameter.Add("quantity", quantity);
            parameter.Add("orderId", orderId);
            parameter.Add("clientOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v1/trade/amend", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey), requestBodyFormat: RequestBodyFormat.Json);
            var result = await _baseClient.SendAsync<BingXEditResult>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrderDetails>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail< BingXFuturesOrderDetails>(result);

            return HttpResult.Ok(result, result.Data.Order);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrderDetails>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrderDetails>(result);

            return HttpResult.Ok(result, result.Data.Order);
        }

        #endregion

        #region Close All Positions

        /// <inheritdoc />
        public async Task<HttpResult<BingXClosePositionsResult>> CloseAllPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/closeAllPositions", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXClosePositionsResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Multiple Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXCancelAllResult>> CancelMultipleOrderAsync(string symbol, IEnumerable<long>? orderIds, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.AddRaw("orderIdList", orderIds?.Select(x => (object)x).ToArray());
            parameters.AddRaw("clientOrderIDList", clientOrderIds?.Select(x => (object)x).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/openApi/swap/v2/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXCancelAllResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXCancelAllResult>> CancelAllOrderAsync(
            string? symbol = null,
            OrderType? orderType = null, 
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("type", orderType);

            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/openApi/swap/v2/trade/allOpenOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXCancelAllResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrderDetails[]>> GetOpenOrdersAsync(
            string? symbol = null,
            OrderType? orderType = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("type", orderType);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/trade/openOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrderDetails[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Get Liquidation Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrderDetails[]>> GetLiquidationOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("currency", settleAsset);
            parameters.Add("autoCloseType", closeType);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/trade/forceOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrderDetails[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrderDetails[]>> GetClosedOrdersAsync(string? symbol = null, long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("currency", settleAsset);
            parameters.Add("orderId", orderId);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/trade/allOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrderDetails[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesUserTrade[]>> GetUserTradesAsync(long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("orderId", orderId);
            parameters.Add("currency", settleAsset);
            parameters.Add("startTs", startTime);
            parameters.Add("endTs", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/trade/allFillOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesUserTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Trades);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesUserTradeDetails[]>> GetUserTradesAsync(string symbol, long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("lastFillId", fromId);
            parameters.Add("currency", settleAsset);
            parameters.Add("pageSize", limit);
            parameters.Add("startTs", startTime);
            parameters.Add("endTs", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/trade/fillHistory", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesUserTradeDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesUserTradeDetails[]>(result);

            return HttpResult.Ok(result, result.Data.Trades);
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/cancelAllAfter", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXCancelAfterResult>(request, parameter, ct).ConfigureAwait(false);
        }

        #endregion

        #region Close Position

        /// <inheritdoc />
        public async Task<HttpResult<BingXClosePositionResult>> ClosePositionAsync(string positionId, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "positionId", positionId }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v1/trade/closePosition", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXClosePositionResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesOrderDetails[]>> GetOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit ?? 500);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/trade/fullOrder", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesOrderDetails[]>(result);

            return HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Get Position And MarginInfo

        /// <inheritdoc />
        public async Task<HttpResult<BingXPositionMarginInfo[]>> GetPositionAndMarginInfoAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/maintMarginRatio", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXPositionMarginInfo[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Position History

        /// <inheritdoc />
        public async Task<HttpResult<BingXPositionHistory[]>> GetPositionHistoryAsync(string symbol, long? positionId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("positionId", positionId);
            parameters.Add("currency", settleAsset);
            parameters.Add("startTs", startTime);
            parameters.Add("endTs", endTime);
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/trade/positionHistory", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXPositionHistoryWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXPositionHistory[]>(result);

            return HttpResult.Ok(result, result.Data.History);
        }

        #endregion

        #region Place Twap Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXTwapOrderId>> PlaceTwapOrderAsync(string symbol, OrderSide orderSide, PositionSide positionSide, PriceType priceType, decimal priceVariance, decimal triggerPrice, int interval, decimal orderQuantity, decimal totalQuantity, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("side", orderSide);
            parameters.Add("positionSide", positionSide);
            parameters.Add("priceType", priceType);
            parameters.AddAsString("priceVariance", priceVariance);
            parameters.AddAsString("triggerPrice", triggerPrice);
            parameters.Add("interval", interval);
            parameters.AddAsString("amountPerOrder", orderQuantity);
            parameters.AddAsString("totalAmount", totalQuantity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v1/twap/order", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrderId>(request, parameters, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange) }
                 }).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Twap Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXTwapOrders>> GetOpenTwapOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/twap/openOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrders>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Twap Orders

        /// <inheritdoc />
        public async Task<HttpResult<BingXTwapOrders>> GetClosedTwapOrdersAsync(string symbol, int page, int pageSize, DateTime startTime, DateTime endTime, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/twap/historyOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrders>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Twap Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXTwapOrder>> GetTwapOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("mainOrderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/twap/orderDetail", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Twap Order

        /// <inheritdoc />
        public async Task<HttpResult<BingXTwapOrder>> CancelTwapOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.AddAsString("mainOrderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v1/twap/cancelOrder", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
