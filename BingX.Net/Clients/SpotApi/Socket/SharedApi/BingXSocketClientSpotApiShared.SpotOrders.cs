using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXSocketClientSpotSharedApi
    {
        #region Subscribe To Spot Order Updates

        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
            => await SubscribeToSpotOrderUpdatesAsync(request, x => handler(x.ToType<SharedSpotOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeSpotOrderOptions SubscribeSpotOrderOptions { get; } = new SubscribeSpotOrderOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return new WebSocketResult<UpdateSubscription>(Exchange, null, validationError);

            var result = await _api.SubscribeToOrderUpdatesAsync(
                update => handler(update.ToType(new[] {
                    new SharedSpotOrderUpdate(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        ParseOrderType(update.Data.Type),
                        update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,                        
                        ParseOrderStatus(update.Data.Status),
                        update.Data.CreateTime)
                    {
                        ClientOrderId = update.Data.ClientOrderId,
                        OrderPrice = update.Data.Price,
                        OrderQuantity = new SharedOrderQuantity(update.Data.Quantity, update.Data.QuoteOrderQuantity == 0 ? null : update.Data.QuoteOrderQuantity, null),
                        QuantityFilled = new SharedOrderQuantity(update.Data.QuantityFilled, update.Data.VolumeFilled, null),
#pragma warning disable CS0618 // Type or member is obsolete
                        Fee = update.Data.Fee.HasValue ? Math.Abs(update.Data.Fee.Value) : null,
                        FeeAsset = update.Data.FeeAsset,
#pragma warning restore CS0618 // Type or member is obsolete
                        UpdateTime = update.Data.UpdateTime,
                        IsTriggerOrder = update.Data.Type == OrderType.StopLimit || update.Data.Type == OrderType.StopMarket || update.Data.Type == OrderType.TriggerLimit || update.Data.Type == OrderType.TriggerMarket,
                        LastTrade = update.Data.LastFillQuantity > 0 ? 
                            new SharedUserTrade(
                                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                                update.Data.Symbol,
                                update.Data.OrderId.ToString(),
                                update.Data.TradeId.ToString(),
                                update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                new SharedOrderQuantity(update.Data.LastFillQuantity !.Value),
                                update.Data.LastFillPrice!.Value,
                                update.Data.UpdateTime!.Value)
                            {
                                ClientOrderId = update.Data.ClientOrderId
                            }
                            : null
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

        private SharedOrderType ParseOrderType(OrderType type)
        {
            if (type == OrderType.Market || type == OrderType.StopMarket)
                return SharedOrderType.Market;

            if (type == OrderType.Limit || type == OrderType.StopLimit)
                return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

    }
}
