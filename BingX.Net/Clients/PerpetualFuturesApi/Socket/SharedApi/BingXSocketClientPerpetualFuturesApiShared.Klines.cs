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
        #region Subscribe To Kline Updates

        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new WebSocketResult<UpdateSubscription>(Exchange, null, ArgumentError.Invalid(nameof(SubscribeKlineRequest.Interval), "Interval not supported"));

            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return new WebSocketResult<UpdateSubscription>(Exchange, null, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToKlineUpdatesAsync(symbol, interval, update =>
            {
                if (update.UpdateType == SocketUpdateType.Snapshot)
                    return;

                foreach (var item in update.Data)
                {
                    handler(update.ToType(new SharedKline(
                        request.Symbol,
                        symbol,
                        item.Timestamp,
                        item.ClosePrice,
                        item.HighPrice, 
                        item.LowPrice,
                        item.OpenPrice, 
                        new SharedOrderQuantity(item.Volume))));
                }
            }, ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
