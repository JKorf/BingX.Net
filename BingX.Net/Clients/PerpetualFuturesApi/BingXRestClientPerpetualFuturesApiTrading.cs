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
using System.Data.Common;
using CryptoExchange.Net.RateLimiting.Guards;
using System.Security.Cryptography;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    internal class BingXRestClientPerpetualFuturesApiTrading : IBingXRestClientPerpetualFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientPerpetualFuturesApi _baseClient;
        private readonly string _brokerId;

        internal BingXRestClientPerpetualFuturesApiTrading(ILogger logger, BingXRestClientPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;

            _brokerId = !string.IsNullOrEmpty(baseClient.ClientOptions.BrokerId) ? baseClient.ClientOptions.BrokerId! : "easytrading";
        }

        #region Get Positions

        /// <inheritdoc />
        public async Task<WebCallResult<BingXPosition[]>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/user/positions", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXPosition[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Place Test Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrder>> PlaceTestOrderAsync(
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
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameter.AddEnum("side", side);
            parameter.AddEnum("type", type);
            parameter.AddEnum("positionSide", positionSide);
            parameter.AddOptional("reduceOnly", reduceOnly?.ToString().ToLowerInvariant());
            parameter.AddOptional("quantity", quantity);
            parameter.AddOptional("price", price);
            parameter.AddOptional("stopPrice", stopPrice);
            parameter.AddOptional("newClientOrderId", clientOrderId);
            parameter.AddOptional("priceRate", priceRate);
            parameter.AddOptionalEnum("timeInForce", timeInForce);
            parameter.AddOptional("closePosition", closePosition?.ToString().ToLowerInvariant());
            parameter.AddOptional("activationPrice", triggerPrice);
            parameter.AddOptional("stopGuaranteed", stopGuaranteed);

            if (stopLossType != null)
            {
                var stopLossParams = new ParameterCollection();
                stopLossParams.AddEnum("type", stopLossType.Value);
                stopLossParams.AddOptional("stopPrice", stopLossStopPrice);
                stopLossParams.AddOptional("price", stopLossPrice);
                stopLossParams.AddOptionalEnum("workingType", stopLossTriggerType);
                stopLossParams.AddOptional("stopGuaranteed", stopLossStopGuaranteed?.ToString().ToLowerInvariant());
                parameter.Add("stopLoss", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(stopLossParams));
            }

            if (takeProfitType != null)
            {
                var takeProfitParams = new ParameterCollection();
                takeProfitParams.AddEnum("type", takeProfitType.Value);
                takeProfitParams.AddOptional("stopPrice", takeProfitStopPrice);
                takeProfitParams.AddOptional("price", takeProfitPrice);
                takeProfitParams.AddOptionalEnum("workingType", takeProfitTriggerType);
                takeProfitParams.AddOptional("stopGuaranteed", takeProfitStopGuaranteed?.ToString().ToLowerInvariant());
                parameter.Add("takeProfit", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(takeProfitParams));
            }

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/order/test", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrderWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<BingXFuturesOrder>(result.Data?.Order);
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrder>> PlaceOrderAsync(
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
            CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameter.AddEnum("side", side);
            parameter.AddEnum("type", type);
            parameter.AddEnum("positionSide", positionSide);
            parameter.AddOptional("reduceOnly", reduceOnly);
            parameter.AddOptional("quantity", quantity);
            parameter.AddOptional("price", price);
            parameter.AddOptional("stopPrice", stopPrice);
            parameter.AddOptional("clientOrderId", clientOrderId);
            parameter.AddOptional("priceRate", priceRate);
            parameter.AddOptionalEnum("timeInForce", timeInForce);
            parameter.AddOptional("closePosition", closePosition);
            parameter.AddOptional("activationPrice", triggerPrice);
            parameter.AddOptional("stopGuaranteed", stopGuaranteed);
            parameter.AddOptionalEnum("workingType", workingType);

            if (stopLossType != null)
            {
                var stopLossParams = new ParameterCollection();
                stopLossParams.AddEnum("type", stopLossType.Value);
                stopLossParams.AddOptional("stopPrice", stopLossStopPrice);
                stopLossParams.AddOptional("price", stopLossPrice);
                stopLossParams.AddOptionalEnum("workingType", stopLossTriggerType);
                stopLossParams.AddOptional("stopGuaranteed", stopLossStopGuaranteed?.ToString().ToLowerInvariant());
                var json = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(stopLossParams);
                json = json.Replace("\u0022", "\"");
                parameter.Add("stopLoss", json);
            }

            if (takeProfitType != null)
            {
                var takeProfitParams = new ParameterCollection();
                takeProfitParams.AddEnum("type", takeProfitType.Value);
                takeProfitParams.AddOptional("stopPrice", takeProfitStopPrice);
                takeProfitParams.AddOptional("price", takeProfitPrice);
                takeProfitParams.AddOptionalEnum("workingType", takeProfitTriggerType);
                takeProfitParams.AddOptional("stopGuaranteed", takeProfitStopGuaranteed?.ToString().ToLowerInvariant());
                var json = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(takeProfitParams);
                json = json.Replace("\u0022", "\"");
                parameter.Add("takeProfit", json);
            }

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey), requestBodyFormat: RequestBodyFormat.Json);
            var result = await _baseClient.SendAsync<BingXFuturesOrderWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<BingXFuturesOrder>(result.Data?.Order);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrder[]>> PlaceMultipleOrderAsync(
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
            var parameter = new ParameterCollection()
            {
                { "batchOrders", new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BingXExchange._serializerContext)).Serialize(orders) }
            };
            parameter.AddOptional("sync", sync);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<BingXFuturesOrder[]>(result.Data?.Orders);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrderDetails>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesOrderDetails>(result.Data?.Order);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrderDetails>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrderDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesOrderDetails>(result.Data?.Order);
        }

        #endregion

        #region Close All Positions

        /// <inheritdoc />
        public async Task<WebCallResult<BingXClosePositionsResult>> CloseAllPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/closeAllPositions", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXClosePositionsResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Multiple Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXCancelAllResult>> CancelMultipleOrderAsync(string symbol, IEnumerable<long>? orderIds, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("orderIdList", orderIds?.Select(x => (object)x).ToArray());
            parameters.AddOptional("clientOrderIDList", clientOrderIds?.Select(x => (object)x).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/openApi/swap/v2/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXCancelAllResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXCancelAllResult>> CancelAllOrderAsync(
            string? symbol = null,
            OrderType? orderType = null, 
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalEnum("type", orderType);

            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/openApi/swap/v2/trade/allOpenOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXCancelAllResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrderDetails[]>> GetOpenOrdersAsync(
            string? symbol = null,
            OrderType? orderType = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalEnum("type", orderType);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/openOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesOrderDetails[]>(result.Data?.Orders);
        }

        #endregion

        #region Get Liquidation Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrderDetails[]>> GetLiquidationOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("currency", settleAsset);
            parameters.AddOptionalEnum("autoCloseType", closeType);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/forceOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesOrderDetails[]>(result.Data?.Orders);
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrderDetails[]>> GetClosedOrdersAsync(string? symbol = null, long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("currency", settleAsset);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/allOrders", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesOrderDetails[]>(result.Data?.Orders);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesUserTrade[]>> GetUserTradesAsync(long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("currency", settleAsset);
            parameters.AddOptionalMilliseconds("startTs", startTime);
            parameters.AddOptionalMilliseconds("endTs", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/allFillOrders", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesUserTrade[]>(result.Data?.Trades);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesUserTradeDetails[]>> GetUserTradesAsync(string symbol, long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("lastFillId", fromId);
            parameters.AddOptional("currency", settleAsset);
            parameters.AddOptional("pageSize", limit);
            parameters.AddOptionalMilliseconds("startTs", startTime);
            parameters.AddOptionalMilliseconds("endTs", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/trade/fillHistory", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesUserTradeDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesUserTradeDetails[]>(result.Data?.Trades);
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/cancelAllAfter", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXCancelAfterResult>(request, parameter, ct).ConfigureAwait(false);
        }

        #endregion

        #region Close Position

        /// <inheritdoc />
        public async Task<WebCallResult<BingXClosePositionResult>> ClosePositionAsync(string positionId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "positionId", positionId }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v1/trade/closePosition", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXClosePositionResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrderDetails[]>> GetOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalString("orderId", orderId);
            parameters.AddOptionalMillisecondsString("startTime", startTime);
            parameters.AddOptionalMillisecondsString("endTime", endTime);
            parameters.Add("limit", limit ?? 500);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/trade/fullOrder", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXFuturesOrderDetails[]>(result.Data?.Orders);
        }

        #endregion

        #region Get Position And MarginInfo

        /// <inheritdoc />
        public async Task<WebCallResult<BingXPositionMarginInfo[]>> GetPositionAndMarginInfoAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/maintMarginRatio", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXPositionMarginInfo[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Position History

        /// <inheritdoc />
        public async Task<WebCallResult<BingXPositionHistory[]>> GetPositionHistoryAsync(string symbol, long? positionId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddOptional("positionId", positionId);
            parameters.AddOptional("currency", settleAsset);
            parameters.AddOptionalMillisecondsString("startTs", startTime);
            parameters.AddOptionalMillisecondsString("endTs", endTime);
            parameters.AddOptional("pageIndex", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/trade/positionHistory", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXPositionHistoryWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXPositionHistory[]>(result.Data?.History);
        }

        #endregion

        #region Place Twap Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTwapOrderId>> PlaceTwapOrderAsync(string symbol, OrderSide orderSide, PositionSide positionSide, PriceType priceType, decimal priceVariance, decimal triggerPrice, int interval, decimal orderQuantity, decimal totalQuantity, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddEnum("side", orderSide);
            parameters.AddEnum("positionSide", positionSide);
            parameters.AddEnum("priceType", priceType);
            parameters.AddString("priceVariance", priceVariance);
            parameters.AddString("triggerPrice", triggerPrice);
            parameters.Add("interval", interval);
            parameters.AddString("amountPerOrder", orderQuantity);
            parameters.AddString("totalAmount", totalQuantity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v1/twap/order", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrderId>(request, parameters, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Twap Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTwapOrders>> GetOpenTwapOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/twap/openOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrders>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Twap Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTwapOrders>> GetClosedTwapOrdersAsync(string symbol, int page, int pageSize, DateTime startTime, DateTime endTime, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.Add("pageIndex", page);
            parameters.Add("pageSize", pageSize);
            parameters.AddMillisecondsString("startTime", startTime);
            parameters.AddMillisecondsString("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/twap/historyOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrders>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Twap Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTwapOrder>> GetTwapOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddString("mainOrderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/twap/orderDetail", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Twap Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTwapOrder>> CancelTwapOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddString("mainOrderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v1/twap/cancelOrder", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXTwapOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
