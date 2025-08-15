using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.Objects.Errors;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class BingXRestClientSpotApiAccount : IBingXRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientSpotApi _baseClient;

        internal BingXRestClientSpotApiAccount(BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<BingXBalance[]>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/account/balance", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXBalanceWrapper>(request, null, ct).ConfigureAwait(false);
            return result.As<BingXBalance[]>(result.Data?.Balances);
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<WebCallResult<BingXDeposit[]>> GetDepositHistoryAsync(string? asset = null, DepositStatus? status = null, string? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("coin", asset);
            parameters.AddOptionalEnum("status", status);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("offset", offset);
            parameters.AddOptional("limit", limit);
            parameters.AddOptional("txId", transactionId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/api/v3/capital/deposit/hisrec", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendRawAsync<BingXDeposit[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public async Task<WebCallResult<BingXWithdrawal[]>> GetWithdrawalHistoryAsync(string? id = null, string? asset = null, string? clientOrderId = null, WithdrawalStatus? status = null, string? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("id", id);
            parameters.AddOptional("coin", asset);
            parameters.AddOptional("withdrawOrderId", clientOrderId);
            parameters.AddOptionalEnum("status", status);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("offset", offset);
            parameters.AddOptional("limit", limit);
            parameters.AddOptional("txId", transactionId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/api/v3/capital/withdraw/history", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendRawAsync<BingXWithdrawal[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<WebCallResult<BingXAsset[]>> GetAssetsAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("coin", asset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/wallets/v1/capital/config/getall", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXAsset[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<WebCallResult<BingXWithdrawResult>> WithdrawAsync(string asset, string address, decimal quantity, AccountType walletType, string? network = null, string? addressTag = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "coin", asset },
                { "address", address },
                { "amount", quantity }
            };
            parameters.AddEnumAsInt("walletType", walletType);
            parameters.AddOptional("network", network);
            parameters.AddOptional("addressTag", addressTag);
            parameters.AddOptional("withdrawOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/wallets/v1/capital/withdraw/apply", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXWithdrawResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Address

        /// <inheritdoc />
        public async Task<WebCallResult<BingXDepositAddresses>> GetDepositAddressAsync(string asset, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "coin", asset }
            };
            parameters.AddOptional("offset", offset);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/wallets/v1/capital/deposit/address", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXDepositAddresses>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTransactionResult>> TransferAsync(TransferType transferType, string asset, decimal quantity, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "asset", asset },
                { "amount", quantity }
            };
            parameters.AddEnum("type", transferType);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/api/v3/post/asset/transfer", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendRawAsync<BingXTransactionResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data == null)
                return result.AsError<BingXTransactionResult>(new ServerError(new ErrorInfo(ErrorType.Unknown, "Transfer failed")));
            return result;
        }

        #endregion
        
        #region Get Transfers

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTransfers>> GetTransfersAsync(TransferType type, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("type", type);
            parameters.AddOptional("tranId", transactionId);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/api/v3/asset/transfer", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendRawAsync<BingXTransfers>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer Internal

        /// <inheritdoc />
        public async Task<WebCallResult<BingXId>> TransferInternalAsync(string asset, AccountIdentifierType targetAccountType, string targetAccount, decimal quantity, AccountType accountType, string? areaCode = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "coin", asset },
                { "amount", quantity },
                { "targetAccount", targetAccount }
            };
            parameters.AddEnum("userAccountType", targetAccountType);
            parameters.AddEnum("walletType", accountType);
            parameters.AddOptional("callingCode", areaCode);
            parameters.AddOptional("transferClientId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/wallets/v1/capital/innerTransfer/apply", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXId>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Internal Transfers

        /// <inheritdoc />
        public async Task<WebCallResult<BingXInternalTransfers>> GetInternalTransfersAsync(string asset, string? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "coin", asset }
            };
            parameters.AddOptional("transferClientId", clientOrderId);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("offset", offset);
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/wallets/v1/capital/innerTransfer/records", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXInternalTransfers>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return new WebCallResult<string>(new NoApiCredentialsError());

            var request = _definitions.GetOrCreate(HttpMethod.Post, "/openApi/user/auth/userDataStream", BingXExchange.RateLimiter.RestAccount1, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendRawAsync<BingXListenKey>(request, null, ct).ConfigureAwait(false);
            return result.As<string>(result.Data?.ListenKey);
        }

        #endregion

        #region Keep Alive User Stream

        /// <inheritdoc />
        public async Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return new WebCallResult(new NoApiCredentialsError());

            var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Put, "/openApi/user/auth/userDataStream", BingXExchange.RateLimiter.RestAccount1, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey), parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Stop User Stream

        /// <inheritdoc />
        public async Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return new WebCallResult(new NoApiCredentialsError());

            var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/openApi/user/auth/userDataStream", BingXExchange.RateLimiter.RestAccount1, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trading Fees

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTradingFees>> GetTradingFeesAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/spot/v1/user/commissionRate", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXTradingFees>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion


        #region Get User Id

        /// <inheritdoc />
        public async Task<WebCallResult<BingXUserId>> GetUserIdAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/account/v1/uid", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXUserId>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trading Fees

        /// <inheritdoc />
        public async Task<WebCallResult<BingXApiKey[]>> GetApiKeyPermissionsAsync(long userId, string? apiKey = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "uid", userId }
            };
            parameters.AddOptional("apiKey", apiKey);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/openApi/account/v1/apiKey/query", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXApiKeyWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BingXApiKey[]>(result.Data?.ApiInfos);
        }

        #endregion
    }
}
