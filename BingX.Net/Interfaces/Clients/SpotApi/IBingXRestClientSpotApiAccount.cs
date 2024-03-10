using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
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
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Query%20Assets" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXBalance>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Deposit%20History(supporting%20network)" /></para>
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="status"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXDeposit>>> GetDepositHistoryAsync(string? asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Withdraw%20History%20(supporting%20network)" /></para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asset"></param>
        /// <param name="clientOrderId"></param>
        /// <param name="status"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXWithdrawal>>> GetWithdrawalHistoryAsync(string? id = null, string? asset = null, string? clientOrderId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get asset info
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#All%20Coins'%20Information" /></para>
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXAsset>>> GetAssetsAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Withdraw funds
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Withdraw" /></para>
        /// </summary>
        /// <param name="asset">The asset</param>
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
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/wallet-api.html#Query%20Main%20Account%20Deposit%20Address" /></para>
        /// </summary>
        /// <param name="asset">The asset</param>
        /// <param name="offset">Offset</param>
        /// <param name="limit">Limit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXDepositAddresses>> GetDepositAddressAsync(string asset, int? offset = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Universal transfer
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#User%20Universal%20Transfer" /></para>
        /// </summary>
        /// <param name="tranferType">Transfer type</param>
        /// <param name="asset">Asset to transfer</param>
        /// <param name="quantity">Quantity to transfer</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXTransactionResult>> TransferAsync(TransferType tranferType, string asset, decimal quantity, CancellationToken ct = default);

        /// <summary>
        /// Get universal transfer history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Query%20User%20Universal%20Transfer%20History%20(USER_DATA)" /></para>
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
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#User%20internal%20transfer" /></para>
        /// </summary>
        /// <param name="asset">Asset</param>
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
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/account-api.html#Main%20Account%20Query%20Inner%20Transfer%20Records" /></para>
        /// </summary>
        /// <param name="asset">Asset</param>
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
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/listenKey.html#generate%20Listen%20Key" /></para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Extend the lifetime of a listenkey with 60 minutes
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/listenKey.html#extend%20Listen%20Key%20Validity%20period" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Delete a listenkey and stop the user data stream
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/socket/listenKey.html#delete%20Listen%20Key" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default);
    }
}
