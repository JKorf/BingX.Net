using BingX.Net.Clients.SpotApi;
using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXRestClientPerpetualFuturesSharedApi
    {
        #region Set Futures Tp Sl

        public SetFuturesTpSlOptions SetFuturesTpSlOptions { get; } = new SetFuturesTpSlOptions(_exchangeName, true);
        async Task<ICallResult<SharedId>> ISetFuturesTpSl.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
            => await SetFuturesTpSlAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = SetFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            HttpResult<BingXFuturesOrder> result;
            if (request.TpSlSide == SharedTpSlSide.TakeProfit)
            {
                result = await _api.Trading.PlaceOrderAsync(
                    request.Symbol!.GetSymbol(FormatSymbol),
                    OrderSide.Buy,
                    FuturesOrderType.TakeProfitMarket,
                    request.PositionMode == SharedPositionMode.OneWay ? PositionSide.Both : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                    stopPrice: request.TriggerPrice,
                    //reduceOnly: request.PositionMode == SharedPositionMode.OneWay ? true : null,
                    closePosition: true,
                    ct: ct).ConfigureAwait(false);
            }
            else
            {
                result = await _api.Trading.PlaceOrderAsync(
                    request.Symbol!.GetSymbol(FormatSymbol),
                    OrderSide.Sell,
                    FuturesOrderType.StopMarket,
                    request.PositionMode == SharedPositionMode.OneWay ? PositionSide.Both : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                    stopPrice: request.TriggerPrice,
                    //reduceOnly: request.PositionMode == SharedPositionMode.OneWay ? true : null,
                    closePosition: true,
                    ct: ct).ConfigureAwait(false);
            }

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        
                
        }

        #endregion

        #region Cancel Futures Tp Sl

        public CancelFuturesTpSlOptions CancelFuturesTpSlOptions { get; } = new CancelFuturesTpSlOptions(_exchangeName, true);
        async Task<ICallResult<bool>> ICancelFuturesTpSl.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
            => await CancelFuturesTpSlAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<bool>> CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<bool>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var id))
                throw new ArgumentException($"Invalid order id");

            var result = await _api.Trading.CancelOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                id,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<bool>(result);

            // Return
            return HttpResult.Ok(result, true);
        
                
        }

        #endregion

    }
}
