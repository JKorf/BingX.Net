using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXRestClientSpotSharedApi
    {
        #region Place Spot Trigger Order

        public PlaceSpotTriggerOrderOptions PlaceSpotTriggerOrderOptions { get; } = new PlaceSpotTriggerOrderOptions(_exchangeName, true);
        async Task<ICallResult<SharedId>> IPlaceSpotTriggerOrder.PlaceSpotTriggerOrderAsync(PlaceSpotTriggerOrderRequest request, CancellationToken ct)
            => await PlaceSpotTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> PlaceSpotTriggerOrderAsync(PlaceSpotTriggerOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.OrderSide == SharedOrderSide.Buy ? OrderSide.Buy: OrderSide.Sell,
                request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit,
                quantity: request.Quantity.QuantityInBaseAsset,
                quoteQuantity: request.Quantity.QuantityInQuoteAsset,
                timeInForce: request.OrderPrice == null ? TimeInForce.ImmediateOrCancel : TimeInForce.GoodTillCanceled,
                price: request.OrderPrice,
                stopPrice: request.TriggerPrice,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        
                
        }

        #endregion

        #region Get Spot Trigger Order

        public GetSpotTriggerOrderOptions GetSpotTriggerOrderOptions { get; } = new GetSpotTriggerOrderOptions(_exchangeName, true);
        async Task<ICallResult<SharedSpotTriggerOrder>> IGetSpotTriggerOrder.GetSpotTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetSpotTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedSpotTriggerOrder>> GetSpotTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTriggerOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var id))
                throw new ArgumentException($"Invalid order id");

            var result = await _api.Trading.GetOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                id,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTriggerOrder>(result);

            var (orderType, orderDirection) = ParseTriggerDirections(result.Data.Type, result.Data.Side);
            // Return
            return HttpResult.Ok(result, new SharedSpotTriggerOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, result.Data.Symbol),
                result.Data.Symbol,
                result.Data.OrderId.ToString(),
                orderType,
                orderDirection,
                ParseTriggerStatus(result.Data),
                result.Data.StopPrice ?? 0,
                result.Data.CreateTime
                )
            {
                PlacedOrderId = result.Data.OrderId.ToString(),
                OrderPrice = result.Data.Price,
                OrderQuantity = new SharedOrderQuantity(result.Data.Quantity, result.Data.QuoteQuantity, null),
                QuantityFilled = new SharedOrderQuantity(result.Data.QuantityFilled, result.Data.ValueFilled, null),
                Fee = Math.Abs(result.Data.Fee),
                FeeAsset = result.Data.FeeAsset,
                UpdateTime = result.Data.UpdateTime,
                AveragePrice = result.Data.AveragePrice == 0 ? null : result.Data.AveragePrice,
                ClientOrderId = result.Data.ClientOrderId
            });
        
                
        }

        #endregion

        private SharedTriggerOrderStatus ParseTriggerStatus(BingXOrderDetails data)
        {
            if (data.Status == OrderStatus.Filled)
                return SharedTriggerOrderStatus.Filled;

            if (data.Status == OrderStatus.Canceled || data.Status == OrderStatus.Failed)
                return SharedTriggerOrderStatus.CanceledOrRejected;

            if (data.Status == OrderStatus.New || data.Status == OrderStatus.PartiallyFilled || data.Status == OrderStatus.Working)
                return SharedTriggerOrderStatus.Active;

            return SharedTriggerOrderStatus.Unknown;
        }

        #region Cancel Spot Trigger Order

        public CancelSpotTriggerOrderOptions CancelSpotTriggerOrderOptions { get; } = new CancelSpotTriggerOrderOptions(_exchangeName, true);
        async Task<ICallResult<SharedId>> ICancelSpotTriggerOrder.CancelSpotTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> CancelSpotTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id"));

            var order = await _api.Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId.ToString()));
        
                
        }

        #endregion

        private (SharedOrderType, SharedTriggerOrderDirection) ParseTriggerDirections(OrderType orderType, OrderSide side)
        {
            if (side == OrderSide.Buy)
            {
                return (
                    orderType == OrderType.StopMarket ? SharedOrderType.Market : SharedOrderType.Limit,
                    SharedTriggerOrderDirection.Enter);
            }
            else
            {
                return (
                    orderType == OrderType.StopMarket ? SharedOrderType.Market : SharedOrderType.Limit,
                    SharedTriggerOrderDirection.Exit);
            }            
        }
    }
}
