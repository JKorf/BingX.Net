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

            _brokerId = !string.IsNullOrEmpty(baseClient.ClientOptions.BrokerId) ? baseClient.ClientOptions.BrokerId! : "TODO";
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
        public async Task<WebCallResult<BingXFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var result = await _baseClient.SendRequestInternal<BingXFuturesOrderWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/order"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result.As<BingXFuturesOrder>(result.Data?.Order);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var result = await _baseClient.SendRequestInternal<BingXFuturesOrderWrapper>(_baseClient.GetUri("/openApi/swap/v2/trade/order"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
            return result.As<BingXFuturesOrder>(result.Data?.Order);
        }

        #endregion
    }
}
