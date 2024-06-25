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

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    public class BingXRestClientPerpetualFuturesApiTrading : IBingXRestClientPerpetualFuturesApiTrading
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
        public async Task<WebCallResult<IEnumerable<BingXPosition>>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/user/positions", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            return await _baseClient.SendAsync<IEnumerable<BingXPosition>>(request, parameters, ct).ConfigureAwait(false);
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

            //TakeProfitStopLossMode? stopLossType = null,
            //decimal? stopLossStopPrice = null,
            //decimal? stopLossPrice = null,
            //TriggerType? stopLossTriggerType = null,
            //bool? stopLossStopGuaranteed = null,

            //TakeProfitStopLossMode? takeProfitType = null,
            //decimal? takeProfitStopPrice = null,
            //decimal? takeProfitPrice = null,
            //TriggerType? takeProfitTriggerType = null,
            //bool? takeProfitStopGuaranteed = null,

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
            parameter.AddOptionalEnum("positionSide", positionSide);
            parameter.AddOptional("reduceOnly", reduceOnly);
            parameter.AddOptional("quantity", quantity);
            parameter.AddOptional("price", price);
            parameter.AddOptional("stopPrice", stopPrice);
            parameter.AddOptional("newClientOrderId", clientOrderId);
            parameter.AddOptional("priceRate", priceRate);
            parameter.AddOptionalEnum("timeInForce", timeInForce);
            parameter.AddOptional("closePosition", closePosition);
            parameter.AddOptional("activationPrice", triggerPrice);
            parameter.AddOptional("stopGuaranteed", stopGuaranteed);

            // TODO how to pass this?
            //if (stopLossType != null)
            //{
            //    var stopLossParams = new ParameterCollection();
            //    stopLossParams.AddEnum("type", stopLossType);
            //    stopLossParams.AddOptional("stopPrice", stopLossStopPrice);
            //    stopLossParams.AddOptional("price", stopLossPrice);
            //    stopLossParams.AddOptionalEnum("workingType", stopLossTriggerType);
            //    stopLossParams.AddOptional("stopGuaranteed", stopLossStopGuaranteed);
            //    parameter.Add("stopLoss", new SystemTextJsonMessageSerializer().Serialize(stopLossParams));
            //}

            //if (takeProfitType != null)
            //{
            //    var stopLossParams = new ParameterCollection();
            //    stopLossParams.AddEnum("type", takeProfitType);
            //    stopLossParams.AddOptional("stopPrice", takeProfitStopPrice);
            //    stopLossParams.AddOptional("price", takeProfitPrice);
            //    stopLossParams.AddOptionalEnum("workingType", takeProfitTriggerType);
            //    stopLossParams.AddOptional("stopGuaranteed", takeProfitStopGuaranteed);
            //    parameter.Add("takeProfit", new SystemTextJsonMessageSerializer().Serialize(stopLossParams));
            //}

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/order/test", BingXExchange.RateLimiter.RestAccount1, 1, true, 5, TimeSpan.FromSeconds(1));
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

            //TakeProfitStopLossMode? stopLossType = null,
            //decimal? stopLossStopPrice = null,
            //decimal? stopLossPrice = null,
            //TriggerType? stopLossTriggerType = null,
            //bool? stopLossStopGuaranteed = null,

            //TakeProfitStopLossMode? takeProfitType = null,
            //decimal? takeProfitStopPrice = null,
            //decimal? takeProfitPrice = null,
            //TriggerType? takeProfitTriggerType = null,
            //bool? takeProfitStopGuaranteed = null,

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
            parameter.AddOptionalEnum("positionSide", positionSide);
            parameter.AddOptional("reduceOnly", reduceOnly);
            parameter.AddOptional("quantity", quantity);
            parameter.AddOptional("price", price);
            parameter.AddOptional("stopPrice", stopPrice);
            parameter.AddOptional("newClientOrderId", clientOrderId);
            parameter.AddOptional("priceRate", priceRate);
            parameter.AddOptionalEnum("timeInForce", timeInForce);
            parameter.AddOptional("closePosition", closePosition);
            parameter.AddOptional("activationPrice", triggerPrice);
            parameter.AddOptional("stopGuaranteed", stopGuaranteed);

            // TODO how to pass this?
            //if (stopLossType != null)
            //{
            //    var stopLossParams = new ParameterCollection();
            //    stopLossParams.AddEnum("type", stopLossType);
            //    stopLossParams.AddOptional("stopPrice", stopLossStopPrice);
            //    stopLossParams.AddOptional("price", stopLossPrice);
            //    stopLossParams.AddOptionalEnum("workingType", stopLossTriggerType);
            //    stopLossParams.AddOptional("stopGuaranteed", stopLossStopGuaranteed);
            //    parameter.Add("stopLoss", Uri.EscapeDataString(new SystemTextJsonMessageSerializer().Serialize(stopLossParams)));
            //}

            //if (takeProfitType != null)
            //{
            //    var stopLossParams = new ParameterCollection();
            //    stopLossParams.AddEnum("type", takeProfitType);
            //    stopLossParams.AddOptional("stopPrice", takeProfitStopPrice);
            //    stopLossParams.AddOptional("price", takeProfitPrice);
            //    stopLossParams.AddOptionalEnum("workingType", takeProfitTriggerType);
            //    stopLossParams.AddOptional("stopGuaranteed", takeProfitStopGuaranteed);
            //    parameter.Add("takeProfit", Uri.EscapeDataString(new SystemTextJsonMessageSerializer().Serialize(stopLossParams)));
            //}

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXFuturesOrderWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<BingXFuturesOrder>(result.Data?.Order);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesOrder>>> PlaceMultipleOrderAsync(
            IEnumerable<BingXFuturesPlaceOrderRequest> orders,
            CancellationToken ct = default)
        {
            var parameter = new ParameterCollection()
            {
                { "batchOrders", new SystemTextJsonMessageSerializer().Serialize(orders) }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersWrapper>(request, parameter, ct, additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<IEnumerable<BingXFuturesOrder>>(result.Data?.Orders);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount1, 1, true, 5, TimeSpan.FromSeconds(1));
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
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/openApi/swap/v2/trade/order", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/closeAllPositions", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
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
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/openApi/swap/v2/trade/batchOrders", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            return await _baseClient.SendAsync<BingXCancelAllResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXCancelAllResult>> CancelAllOrderAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/openApi/swap/v2/trade/allOpenOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, 5, TimeSpan.FromSeconds(1));
            return await _baseClient.SendAsync<BingXCancelAllResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesOrderDetails>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/openOrders", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXFuturesOrderDetails>>(result.Data?.Orders);
        }

        #endregion

        #region Get Liquidation Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesOrderDetails>>> GetLiquidationOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalEnum("autoCloseType", closeType);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/forceOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, 10, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXFuturesOrderDetails>>(result.Data?.Orders);
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesOrderDetails>>> GetClosedOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/allOrders", BingXExchange.RateLimiter.RestAccount2, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXFuturesOrdersDetailsWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXFuturesOrderDetails>>(result.Data?.Orders);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesUserTrade>>> GetUserTradesAsync(long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptionalMilliseconds("startTs", startTime);
            parameters.AddOptionalMilliseconds("endTs", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v2/trade/allFillOrders", BingXExchange.RateLimiter.RestAccount1, 1, true, 5, TimeSpan.FromSeconds(1));
            var result = await _baseClient.SendAsync<BingXFuturesUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BingXFuturesUserTrade>>(result.Data?.Trades);
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v2/trade/cancelAllAfter", BingXExchange.RateLimiter.RestAccount1, 1, true, 2, TimeSpan.FromSeconds(1));
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/swap/v1/trade/closePosition", BingXExchange.RateLimiter.RestAccount1, 1, true, 5, TimeSpan.FromSeconds(1));
            return await _baseClient.SendAsync<BingXClosePositionResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Position And MarginInfo

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXPositionMarginInfo>>> GetPositionAndMarginInfoAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/swap/v1/maintMarginRatio", BingXExchange.RateLimiter.RestAccount1, 1, true, 5, TimeSpan.FromSeconds(1));
            return await _baseClient.SendAsync<IEnumerable<BingXPositionMarginInfo>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
