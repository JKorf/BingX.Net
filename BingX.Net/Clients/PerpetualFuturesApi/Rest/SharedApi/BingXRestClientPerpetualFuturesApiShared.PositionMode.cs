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
        #region Get Position Mode

        public SharedPositionModeSelection PositionModeSettingType => SharedPositionModeSelection.PerAccount;

        public GetPositionModeOptions GetPositionModeOptions { get; } = new GetPositionModeOptions(_exchangeName);
        async Task<ICallResult<SharedPositionModeResult>> IGetPositionMode.GetPositionModeAsync(GetPositionModeRequest request, CancellationToken ct)
            => await GetPositionModeAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedPositionModeResult>> GetPositionModeAsync(GetPositionModeRequest request, CancellationToken ct)
        {
            var validationError = GetPositionModeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPositionModeResult>(Exchange, validationError);

            var result = await _api.Account.GetPositionModeAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPositionModeResult>(result);

            return HttpResult.Ok(result, new SharedPositionModeResult(result.Data.PositionMode == PositionMode.DualPositionMode ? SharedPositionMode.HedgeMode : SharedPositionMode.OneWay));
        
                
        }

        #endregion

        #region Set Position Mode

        public SetPositionModeOptions SetPositionModeOptions { get; } = new SetPositionModeOptions(_exchangeName);
        async Task<ICallResult<SharedPositionModeResult>> ISetPositionMode.SetPositionModeAsync(SetPositionModeRequest request, CancellationToken ct)
            => await SetPositionModeAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedPositionModeResult>> SetPositionModeAsync(SetPositionModeRequest request, CancellationToken ct)
        {
            var validationError = SetPositionModeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPositionModeResult>(Exchange, validationError);

            var result = await _api.Account.SetPositionModeAsync(request.PositionMode == SharedPositionMode.HedgeMode ? PositionMode.DualPositionMode : PositionMode.SinglePositionMode, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPositionModeResult>(result);

            return HttpResult.Ok(result, new SharedPositionModeResult(request.PositionMode));
        
                
        }

        #endregion
    }
}
