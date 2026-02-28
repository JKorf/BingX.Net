using BingX.Net.Enums;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#Query%20account%20data" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v3/user/balance
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesBalance[]>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get income history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#Get%20Account%20Profit%20and%20Loss%20Fund%20Flow" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/user/income
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="incomeType">Filter by income type</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXIncome[]>> GetIncomesAsync(string? symbol = null, IncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get trading fee rates
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#User%20fee%20rate" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/user/commissionRate
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesTradingFees>> GetTradingFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Generate a listen key used for subscribing to user data streams with the socket client
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#Query%20Trading%20Commission%20Rate" /><br />
        /// Endpoint:<br />
        /// POST /openApi/user/auth/userDataStream
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Extend the lifetime of a listenkey with 60 minutes
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/listenKey.html#extend%20Listen%20Key%20Validity%20period" /><br />
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
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/socket/listenKey.html#delete%20Listen%20Key" /><br />
        /// Endpoint:<br />
        /// DELETE /openApi/user/auth/userDataStream
        /// </para>
        /// </summary>
        /// <param name="listenKey">Listen key</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Get the current margin mode for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Margin%20Type" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/trade/marginType
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXMarginMode>> GetMarginModeAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Change the margin mode for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Change%20Margin%20Type" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/marginType
        /// </para>
        /// </summary>=
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="marginMode">New margin mode</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> SetMarginModeAsync(string symbol, MarginMode marginMode, CancellationToken ct = default);

        /// <summary>
        /// Get the current leverage settings for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Leverage" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/trade/leverage
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLeverage>> GetLeverageAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Set new leverage settings for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Switch%20Leverage" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/leverage
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="side">Position side. In the One-way mode, only supports BOTH.</param>
        /// <param name="leverage">Leverage</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXLeverageResult>> SetLeverageAsync(string symbol, PositionSide side, int leverage, CancellationToken ct = default);

        /// <summary>
        /// Adjust isolated margin for a position
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Set%20Leverage" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/positionMargin
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="quantity">Quantity to adjust with</param>
        /// <param name="direction">Direction</param>
        /// <param name="side">Position side</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> AdjustIsolatedMarginAsync(string symbol, decimal quantity, AdjustDirection direction, PositionSide side, CancellationToken ct = default);

        /// <summary>
        /// Get current position mode
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20position%20mode" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/positionSide/dual
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXPositionMode>> GetPositionModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Set position mode
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Set%20Position%20Mode" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v1/positionSide/dual
        /// </para>
        /// </summary>
        /// <param name="positionMode">Position mode</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXPositionMode>> SetPositionModeAsync(PositionMode positionMode, CancellationToken ct = default);

        /// <summary>
        /// Get history of margin changes
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Isolated%20Margin%20Change%20History" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/positionMargin/history
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="positionId">Filter by positionId</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXMarginHistory>> GetIsolatedMarginChangeHistoryAsync(string positionId, string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Apply for receiving VST assets. Only available on the VST environment
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Apply%20VST" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v1/trade/getVst
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXAmount>> ApplyForVSTAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Set multi asset mode
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Switch%20Multi-Assets%20Mode" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v1/trade/assetMode
        /// </para>
        /// </summary>
        /// <param name="assetMode">Multi asset mode</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXMultiAssetMode>> SetMultiAssetModeAsync(MultiAssetMode assetMode, CancellationToken ct = default);

        /// <summary>
        /// Get current multi asset mode
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Multi-Assets%20Mode" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/trade/assetMode
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXMultiAssetMode>> GetMultiAssetModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get multi asset mode rules
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Multi-Assets%20Rules" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/trade/multiAssetsRules
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXMultiAssetRules[]>> GetMultiAssetRulesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get multi assets margin
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Multi-Assets%20Margin" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/user/marginAssets
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXMarginAsset[]>> GetMultiAssetsMarginAsync(CancellationToken ct = default);

    }
}
