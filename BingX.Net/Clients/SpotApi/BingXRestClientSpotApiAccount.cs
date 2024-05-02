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

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BingXRestClientSpotApiAccount : IBingXRestClientSpotApiAccount
    {
        private readonly BingXRestClientSpotApi _baseClient;

        internal BingXRestClientSpotApiAccount(BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXBalance>>> GetBalancesAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternal<BingXBalanceWrapper>(_baseClient.GetUri("/openApi/spot/v1/account/balance"), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result.As<IEnumerable<BingXBalance>>(result.Data?.Balances);
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXDeposit>>> GetDepositHistoryAsync(string? asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("coin", asset);
            parameters.AddOptionalEnum("status", status);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("offset", offset);
            parameters.AddOptional("limit", limit);
            return await _baseClient.SendRequestInternalRaw<IEnumerable<BingXDeposit>>(_baseClient.GetUri("/openApi/api/v3/capital/deposit/hisrec"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXWithdrawal>>> GetWithdrawalHistoryAsync(string? id = null, string? asset = null, string? clientOrderId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default)
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
            return await _baseClient.SendRequestInternalRaw<IEnumerable<BingXWithdrawal>>(_baseClient.GetUri("/openApi/api/v3/capital/withdraw/history"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXAsset>>> GetAssetsAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("coin", asset);
            return await _baseClient.SendRequestInternal<IEnumerable<BingXAsset>>(_baseClient.GetUri("/openApi/wallets/v1/capital/config/getall"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXWithdrawResult>(_baseClient.GetUri("/openApi/wallets/v1/capital/withdraw/apply"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXDepositAddresses>(_baseClient.GetUri("/openApi/wallets/v1/capital/deposit/address"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<BingXTransactionResult>> TransferAsync(TransferType tranferType, string asset, decimal quantity, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "asset", asset },
                { "amount", quantity }
            };
            parameters.AddEnum("type", tranferType);
            var result = await _baseClient.SendRequestInternalRaw<BingXTransactionResult>(_baseClient.GetUri("/openApi/api/v3/post/asset/transfer"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data == null)
                return result.AsError<BingXTransactionResult>(new ServerError("Transfer failed"));
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
            return await _baseClient.SendRequestInternalRaw<BingXTransfers>(_baseClient.GetUri("/openApi/api/v3/asset/transfer"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXId>(_baseClient.GetUri("/openApi/wallets/v1/capital/innerTransfer/apply"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXInternalTransfers>(_baseClient.GetUri("/openApi/wallets/v1/capital/innerTransfer/records"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternalRaw<BingXListenKey>(_baseClient.GetUri("/openApi/user/auth/userDataStream"), HttpMethod.Post, ct, null, false).ConfigureAwait(false);
            return result.As<string>(result.Data?.ListenKey);
        }

        #endregion

        #region Keep Alive User Stream

        /// <inheritdoc />
        public async Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };
            return await _baseClient.SendRequestInternal(_baseClient.GetUri("/openApi/user/auth/userDataStream"), HttpMethod.Put, ct, parameters, false).ConfigureAwait(false);
        }

        #endregion

        #region Stop User Stream

        /// <inheritdoc />
        public async Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };
            return await _baseClient.SendRequestInternal(_baseClient.GetUri("/openApi/user/auth/userDataStream"), HttpMethod.Delete, ct, parameters, false).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal<BingXTradingFees>(_baseClient.GetUri("/openApi/spot/v1/user/commissionRate"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion
    }
}
