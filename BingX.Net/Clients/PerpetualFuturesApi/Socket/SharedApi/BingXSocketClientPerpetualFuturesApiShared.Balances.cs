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
        #region Subscribe To Balance Updates

        public SubscribeBalanceOptions SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return new WebSocketResult<UpdateSubscription>(Exchange, null, validationError);

            var result = await _api.SubscribeToUserDataUpdatesAsync(
                onAccountUpdate: update => handler(update.ToType(update.Data.Update.Balances.Select(x => 
                    new SharedBalance(
                        SupportedTradingModes, 
                        x.Asset,
                        x.BalanceExIsolatedMargin,
                        x.Balance)).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

    }
}
