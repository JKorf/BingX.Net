using BingX.Net.Enums;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Interfaces.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// BingX futures account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IBingXRestClientPerpetualFuturesApiAccount
    {
        /// <summary>
        /// Get balance info
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#Get%20Perpetual%20Swap%20Account%20Asset%20Information" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesBalance>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get income history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#Get%20Account%20Profit%20and%20Loss%20Fund%20Flow" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="incomeType">Filter by income type</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXIncome>>> GetIncomesAsync(string? symbol = null, IncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get trading fee rates
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#User%20fee%20rate" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesTradingFees>> GetTradingFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Generate a listen key used for subscribing to user data streams with the socket client
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/listenKey.html#generate%20Listen%20Key" /></para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Extend the lifetime of a listenkey with 60 minutes
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/listenKey.html#extend%20Listen%20Key%20Validity%20period" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Delete a listenkey and stop the user data stream
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/listenKey.html#delete%20Listen%20Key" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Get the current margin mode for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Margin%20Mode" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXMarginMode>> GetMarginModeAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Change the margin mode for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Switch%20Margin%20Mode" /></para>
        /// </summary>=
        /// <param name="symbol">Symbol</param>
        /// <param name="marginMode">New margin mode</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXMarginMode>> SetMarginModeAsync(string symbol, MarginMode marginMode, CancellationToken ct = default);

        /// <summary>
        /// Get the current leverage setrings for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Leverage" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLeverage>> GetLeverageAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Set new leverage settings for a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Switch%20Leverage" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="side">Position side. In the One-way mode, only supports BOTH.</param>
        /// <param name="leverage">Leverage</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLeverageResult>> SetLeverageAsync(string symbol, PositionSide side, int leverage, CancellationToken ct = default);

        /// <summary>
        /// Adjust isolated margin for a position
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Switch%20Leverage" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="quantity">Quantity to adjust with</param>
        /// <param name="direction">Direction</param>
        /// <param name="side">Position side</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXIsolatedMarginResult>> AdjustIsolatedMarginAsync(string symbol, decimal quantity, AdjustDirection direction, PositionSide side, CancellationToken ct = default);

        /// <summary>
        /// Get current position mode
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Get%20Position%20Mode" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXPositionMode>> GetPositionModeAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Set position mode
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Set%20Position%20Mode" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="positionMode">Position mode</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXPositionMode>> SetPositionModeAsync(string symbol, PositionMode positionMode, CancellationToken ct = default);
    }
}
