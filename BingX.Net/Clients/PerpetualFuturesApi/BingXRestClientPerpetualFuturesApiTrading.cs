using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using System.Threading;
using System.Net.Http;
using System.Security.Cryptography;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    public class BingXRestClientPerpetualFuturesApiTrading : IBingXRestClientPerpetualFuturesApiTrading
    {
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
            return await _baseClient.SendRequestInternal<IEnumerable<BingXPosition>>(_baseClient.GetUri("/openApi/swap/v2/user/positions"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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

            if (stopLossType != null)
            {
                var stopLossParams = new ParameterCollection();
                stopLossParams.AddEnum("type", stopLossType);
                stopLossParams.AddOptional("stopPrice", stopLossStopPrice);
                stopLossParams.AddOptional("price", stopLossPrice);
                stopLossParams.AddOptionalEnum("workingType", stopLossTriggerType);
                stopLossParams.AddOptional("stopGuaranteed", stopLossStopGuaranteed);
                parameter.Add("stopLoss", new SystemTextJsonMessageSerializer().Serialize(stopLossParams));
            }

            if (takeProfitType != null)
            {
                var stopLossParams = new ParameterCollection();
                stopLossParams.AddEnum("type", takeProfitType);
                stopLossParams.AddOptional("stopPrice", takeProfitStopPrice);
                stopLossParams.AddOptional("price", takeProfitPrice);
                stopLossParams.AddOptionalEnum("workingType", takeProfitTriggerType);
                stopLossParams.AddOptional("stopGuaranteed", takeProfitStopGuaranteed);
                parameter.Add("takeProfit", new SystemTextJsonMessageSerializer().Serialize(stopLossParams));
            }

            var result = await _baseClient.SendRequestInternal<BingXFuturesOrderWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/order/test"), HttpMethod.Post, ct, parameter, true,
                 additionalHeaders: new Dictionary<string, string>
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

            if (stopLossType != null)
            {
                var stopLossParams = new ParameterCollection();
                stopLossParams.AddEnum("type", stopLossType);
                stopLossParams.AddOptional("stopPrice", stopLossStopPrice);
                stopLossParams.AddOptional("price", stopLossPrice);
                stopLossParams.AddOptionalEnum("workingType", stopLossTriggerType);
                stopLossParams.AddOptional("stopGuaranteed", stopLossStopGuaranteed);
                parameter.Add("stopLoss", new SystemTextJsonMessageSerializer().Serialize(stopLossParams));
            }

            if (takeProfitType != null)
            {
                var stopLossParams = new ParameterCollection();
                stopLossParams.AddEnum("type", takeProfitType);
                stopLossParams.AddOptional("stopPrice", takeProfitStopPrice);
                stopLossParams.AddOptional("price", takeProfitPrice);
                stopLossParams.AddOptionalEnum("workingType", takeProfitTriggerType);
                stopLossParams.AddOptional("stopGuaranteed", takeProfitStopGuaranteed);
                parameter.Add("takeProfit", new SystemTextJsonMessageSerializer().Serialize(stopLossParams));
            }

            var result = await _baseClient.SendRequestInternal<BingXFuturesOrderWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/order"), HttpMethod.Post, ct, parameter, true,
                 additionalHeaders: new Dictionary<string, string>
                 {
                     { "X-SOURCE-KEY", _brokerId }
                 }).ConfigureAwait(false);
            return result.As<BingXFuturesOrder>(result.Data?.Order);
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
            var result = await _baseClient.SendRequestInternal<BingXFuturesOrderDetailsWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/order"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            var result = await _baseClient.SendRequestInternal<BingXFuturesOrderDetailsWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/order"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
            return result.As<BingXFuturesOrderDetails>(result.Data?.Order);
        }

        #endregion

        #region Close All Positions

        /// <inheritdoc />
        public async Task<WebCallResult<BingXClosePositionsResult>> CloseAllPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            return await _baseClient.SendRequestInternal<BingXClosePositionsResult>(_baseClient.GetUri("/openApi/swap/v2/trade/closeAllPositions"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXCancelAllResult>> CancelAllOrderAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            return await _baseClient.SendRequestInternal<BingXCancelAllResult>(_baseClient.GetUri("/openApi/swap/v2/trade/allOpenOrders"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesOrderDetails>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            var result = await _baseClient.SendRequestInternal<BingXFuturesOrdersDetailsWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/openOrders"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            var result = await _baseClient.SendRequestInternal<BingXFuturesOrdersDetailsWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/forceOrders"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result.As<IEnumerable<BingXFuturesOrderDetails>>(result.Data?.Orders);
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXFuturesOrderDetails>>> GetClosedOrderAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);
            var result = await _baseClient.SendRequestInternal<BingXFuturesOrdersDetailsWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/allOrders"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            var result = await _baseClient.SendRequestInternal<BingXFuturesUserTradeWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/allFillOrders"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result.As<IEnumerable<BingXFuturesUserTrade>>(result.Data?.Trades);
        }

        #endregion
    }
}
