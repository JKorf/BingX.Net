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
        #region Subscribe To Position Updates

        public SubscribePositionOptions SubscribePositionOptions { get; } = new SubscribePositionOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<DataEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return new WebSocketResult<UpdateSubscription>(Exchange, null, validationError);

            var result = await _api.SubscribeToUserDataUpdatesAsync(
                onAccountUpdate: update => handler(update.ToType(update.Data.Update.Positions.Select(x => 
                    new SharedPosition(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        new SharedOrderQuantity(x.Size), 
                        update.Data.EventTime)
                    {
                        AverageOpenPrice = x.EntryPrice,
                        PositionMode = SharedPositionMode.HedgeMode,
                        PositionSide = x.Side == Enums.TradeSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                        UnrealizedPnl = x.UnrealizedPnl
                    }).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

    }
}
