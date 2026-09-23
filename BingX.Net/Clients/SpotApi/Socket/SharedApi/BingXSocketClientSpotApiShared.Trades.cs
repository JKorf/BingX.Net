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
        #region Subscribe To Trade Updates

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return new WebSocketResult<UpdateSubscription>(Exchange, null, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToTradeUpdatesAsync(symbol, update => handler(update.ToType(new[] {
                new SharedTrade(request.Symbol, symbol, new SharedOrderQuantity(update.Data.Quantity), update.Data.Price, update.Data.TradeTime)
            {
                Side = update.Data.BuyerIsMaker ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            } })), ct).ConfigureAwait(false);

            return result;
        }

        #endregion

    }
}
