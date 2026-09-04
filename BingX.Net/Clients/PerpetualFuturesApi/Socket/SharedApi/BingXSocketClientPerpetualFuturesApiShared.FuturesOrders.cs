using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXSocketClientPerpetualFuturesSharedApi
    {
        #region Subscribe To Futures Order Updates

        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
            => await SubscribeToFuturesOrderUpdatesAsync(request, x => handler(x.ToType<SharedFuturesOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeFuturesOrderOptions SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return new WebSocketResult<UpdateSubscription>(Exchange, null, validationError);

            var result = await _api.SubscribeToUserDataUpdatesAsync(
                onOrderUpdate: update => handler(update.ToType(new[] {
                    new SharedFuturesOrderUpdate(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        ParseOrderType(update.Data),
                        update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        ParseOrderStatus(update.Data.Status),
                        update.Data.UpdateTime)
                    {
                        ClientOrderId = update.Data.ClientOrderId,
                        OrderPrice = update.Data.Price,
                        OrderQuantity = new SharedOrderQuantity(update.Data.Quantity, null, update.Data.Quantity),
                        QuantityFilled = new SharedOrderQuantity(update.Data.QuantityFilled, update.Data.VolumeFilled, update.Data.QuantityFilled),
                        AveragePrice = update.Data.AveragePrice == 0 ? null : update.Data.AveragePrice,
                        PositionSide = update.Data.PositionSide == Enums.PositionSide.Long ? SharedPositionSide.Long : update.Data.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : null,
#pragma warning disable CS0618 // Type or member is obsolete
                        Fee = update.Data.Fee == null ? null :Math.Abs(update.Data.Fee.Value),
                        FeeAsset = update.Data.FeeAsset,
#pragma warning restore CS0618 // Type or member is obsolete
                        UpdateTime = update.Data.UpdateTime,
                        ReduceOnly = update.Data.ReduceOnly,
                        TriggerPrice = update.Data.TriggerPrice,
                        IsTriggerOrder = update.Data.TriggerPrice > 0,
                        IsCloseOrder = (update.Data.Type == Enums.FuturesOrderType.TakeProfitMarket || update.Data.Type == Enums.FuturesOrderType.TakeProfitLimit || update.Data.Type == Enums.FuturesOrderType.StopLimit || update.Data.Type == Enums.FuturesOrderType.StopMarket)
                                            && (update.Data.Quantity == null || update.Data.Quantity == 0)
                    }
                })),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == Enums.OrderStatus.Canceled || status == OrderStatus.Failed)
                return SharedOrderStatus.Canceled;
            if (status == Enums.OrderStatus.New || status == Enums.OrderStatus.Pending || status == Enums.OrderStatus.PartiallyFilled || status == OrderStatus.Working)
                return SharedOrderStatus.Open;
            if (status == OrderStatus.Filled)
                return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(BingXFuturesOrderUpdate data)
        {
            if (data.Type == Enums.FuturesOrderType.Market
                || data.Type == Enums.FuturesOrderType.StopMarket
                || data.Type == Enums.FuturesOrderType.TriggerMarket
                || data.Type == Enums.FuturesOrderType.TakeProfitMarket)
            {
                return SharedOrderType.Market;
            }

            if (data.Type == Enums.FuturesOrderType.Limit
                || data.Type == Enums.FuturesOrderType.StopLimit
                || data.Type == Enums.FuturesOrderType.TakeProfitLimit)
            {
                return SharedOrderType.Limit;
            }

            return SharedOrderType.Other;
        }

    }
}
