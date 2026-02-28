using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX Spot account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IBingXRestClientSpotApiAccount
    {
        /// <summary>
        /// Get balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Query%20Assets" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/account/balance
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXBalance[]>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get funding account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Query%20Fund%20Account%20Assets" /><br />
        /// Endpoint:<br />
        /// GET /openApi/fund/v1/account/balance
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXBalance[]>> GetFundingBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Deposit%20records" /><br />
        /// Endpoint:<br />
        /// GET /openApi/api/v3/capital/deposit/hisrec
        /// </para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="status">Filter by status</param>
        /// <param name="transactionId">Filter by transaction id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXDeposit[]>> GetDepositHistoryAsync(string? asset = null, DepositStatus? status = null, string? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Withdraw%20records" /><br />
        /// Endpoint:<br />
        /// GET /openApi/api/v3/capital/withdraw/history
        /// </para>
        /// </summary>
        /// <param name="id">Filter by id</param>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="transactionId">Filter by transaction id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXWithdrawal[]>> GetWithdrawalHistoryAsync(string? id = null, string? asset = null, string? clientOrderId = null, WithdrawalStatus? status = null, string? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get asset info
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Query%20Assets" /><br />
        /// Endpoint:<br />
        /// GET /openApi/wallets/v1/capital/config/getall
        /// </para>
        /// </summary>
        /// <param name="asset">Filter by asset name, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXAsset[]>> GetAssetsAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Withdraw funds
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Withdraw" /><br />
        /// Endpoint:<br />
        /// POST /openApi/wallets/v1/capital/withdraw/apply
        /// </para>
        /// </summary>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="address">Address to withdraw to</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="walletType">Wallet type</param>
        /// <param name="network">Network to use</param>
        /// <param name="addressTag">Address tag</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXWithdrawResult>> WithdrawAsync(string asset, string address, decimal quantity, AccountType walletType, string? network = null, string? addressTag = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit addresses
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Main%20Account%20Deposit%20Address" /><br />
        /// Endpoint:<br />
        /// GET /openApi/wallets/v1/capital/deposit/address
        /// </para>
        /// </summary>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="offset">Offset</param>
        /// <param name="limit">Limit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXDepositAddresses>> GetDepositAddressAsync(string asset, int? offset = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Universal transfer
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Asset%20Transfer%20New" /><br />
        /// Endpoint:<br />
        /// POST /openApi/api/asset/v1/transfer
        /// </para>
        /// </summary>
        /// <param name="asset">Asset to transfer, for example `ETH`</param>
        /// <param name="quantity">Quantity to transfer</param>
        /// <param name="fromAccount">From account type</param>
        /// <param name="toAccount">To account type</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXTransactionResult>> TransferAsync(string asset, decimal quantity, TransferAccountType fromAccount, TransferAccountType toAccount, CancellationToken ct = default);

        /// <summary>
        /// Get universal transfer history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Asset%20transfer%20records" /><br />
        /// Endpoint:<br />
        /// GET /openApi/api/v3/asset/transfer
        /// </para>
        /// </summary>
        /// <param name="type">Transaction type</param>
        /// <param name="transactionId">Filter by id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXTransfers>> GetTransfersAsync(TransferType type, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer internal to an account on the BingX platform
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Main%20Accoun%20internal%20transfer" /><br />
        /// Endpoint:<br />
        /// POST /openApi/wallets/v1/capital/innerTransfer/apply
        /// </para>
        /// </summary>
        /// <param name="asset">Asset, for example `ETH`</param>
        /// <param name="targetAccountType">Target account type</param>
        /// <param name="targetAccount">Target account</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="accountType">Source account type</param>
        /// <param name="areaCode">Area code for telephone, required when targetAccountType is PhoneNumber</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXId>> TransferInternalAsync(string asset, AccountIdentifierType targetAccountType, string targetAccount, decimal quantity, AccountType accountType, string? areaCode = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get internal transfer history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Main%20account%20internal%20transfer%20records" /><br />
        /// Endpoint:<br />
        /// GET /openApi/wallets/v1/capital/innerTransfer/records
        /// </para>
        /// </summary>
        /// <param name="asset">Asset, for example `ETH`</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Offset</param>
        /// <param name="limit">Limit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXInternalTransfers>> GetInternalTransfersAsync(string asset, string? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Generate a listen key used for subscribing to user data streams with the socket client
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/listenKey.html#generate%20Listen%20Key" /><br />
        /// Endpoint:<br />
        /// POST /openApi/user/auth/userDataStream
        /// </para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Extend the lifetime of a listenkey with 60 minutes
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/listenKey.html#extend%20Listen%20Key%20Validity%20period" /><br />
        /// Endpoint:<br />
        /// PUT /openApi/user/auth/userDataStream
        /// </para>
        /// </summary>
        /// <param name="listenKey">Listen key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Delete a listenkey and stop the user data stream
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/listenKey.html#delete%20Listen%20Key" /><br />
        /// Endpoint:<br />
        /// DELETE /openApi/user/auth/userDataStream
        /// </para>
        /// </summary>
        /// <param name="listenKey">Listen key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Get trading fees for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Query%20Trading%20Commission%20Rate" /><br />
        /// Endpoint:<br />
        /// GET /openApi/spot/v1/user/commissionRate
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXTradingFees>> GetTradingFeesAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get user id
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/common/sub-account#Query%20account%20uid" /><br />
        /// Endpoint:<br />
        /// GET /openApi/account/v1/uid
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXUserId>> GetUserIdAsync(CancellationToken ct = default);

        /// <summary>
        /// Get API key permissions and info
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/common/sub-account#Query%20the%20API%20Key%20of%20a%20sub-account" /><br />
        /// Endpoint:<br />
        /// GET /openApi/account/v1/apiKey/query
        /// </para>
        /// </summary>
        /// <param name="userId">The user id, can be retrieved with <see cref="GetUserIdAsync(CancellationToken)" /> </param>
        /// <param name="apiKey">Filter by API key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXApiKey[]>> GetApiKeyPermissionsAsync(long userId, string? apiKey = null, CancellationToken ct = default);
    }
}
