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
        #region Transfer

        public TransferOptions TransferOptions { get; } = new TransferOptions(_exchangeName, [
            SharedAccountType.Funding,
            SharedAccountType.Spot,
            SharedAccountType.PerpetualLinearFutures,
            SharedAccountType.PerpetualInverseFutures
            ])
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<TransferRequest>.NotSupported(x => x.FromSymbol),
                RequestParameterRuleOverride<TransferRequest>.NotSupported(x => x.ToSymbol),
                ]
        };
        async Task<ICallResult<SharedId>> ITransfer.TransferAsync(TransferRequest request, CancellationToken ct)
            => await TransferAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> TransferAsync(TransferRequest request, CancellationToken ct)
        {
            var validationError = TransferOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var fromAccount = GetTransferType(request.FromAccountType);
            var toAccount = GetTransferType(request.ToAccountType);
            if (fromAccount == null || toAccount == null)
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("To/From AccountType", "invalid to/from account combination"));

            // Get data
            var transfer = await _api.Account.TransferAsync(
                request.Asset,
                request.Quantity,
                fromAccount.Value,
                toAccount.Value,
                ct: ct).ConfigureAwait(false);
            if (!transfer.Success)
                return HttpResult.Fail<SharedId>(transfer);

            return HttpResult.Ok(transfer, new SharedId(transfer.Data.TransactionId.ToString()));
        
                
        }

        #endregion

        private TransferAccountType? GetTransferType(SharedAccountType type)
        {
            if (type == SharedAccountType.Funding) return TransferAccountType.Funding;
            if (type == SharedAccountType.Spot) return TransferAccountType.Spot;
            if (type == SharedAccountType.PerpetualLinearFutures) return TransferAccountType.UsdtPerpetualFutures;
            if (type == SharedAccountType.PerpetualInverseFutures) return TransferAccountType.CoinPerpetualFutures;
            return null;
        }

    }
}
