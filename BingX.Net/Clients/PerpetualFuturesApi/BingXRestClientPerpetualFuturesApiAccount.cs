using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Objects.Internal;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    public class BingXRestClientPerpetualFuturesApiAccount : IBingXRestClientPerpetualFuturesApiAccount
    {
        private readonly BingXRestClientPerpetualFuturesApi _baseClient;

        internal BingXRestClientPerpetualFuturesApiAccount(BingXRestClientPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesBalance>> GetBalancesAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternal<BingXFuturesBalanceWrapper>(_baseClient.GetUri("/openApi/swap/v2/user/balance"), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result.As<BingXFuturesBalance>(result.Data?.Balance);
        }

        #endregion

        #region Get Incomes

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BingXIncome>>> GetIncomesAsync(string? symbol = null, IncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalEnum("incomeType", incomeType);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("limit", limit);
            var result = await _baseClient.SendRequestInternal<IEnumerable<BingXIncome>>(_baseClient.GetUri("/openApi/swap/v2/user/income"), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            if (result && result.Data == null)
            {
                // No items returns null; return empty array instead
                return result.As<IEnumerable<BingXIncome>>(Array.Empty<BingXIncome>());
            }
            return result;
        }

        #endregion

        #region Get Trading Fees

        /// <inheritdoc />
        public async Task<WebCallResult<BingXFuturesTradingFees>> GetTradingFeesAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternal<BingXFuturesTradingFeesWrapper>(_baseClient.GetUri("/openApi/swap/v2/user/commissionRate"), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result.As<BingXFuturesTradingFees>(result.Data?.Rates);
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternalRaw<BingXListenKey>(_baseClient.GetUri("/openApi/user/auth/userDataStream"), HttpMethod.Post, ct, null, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal(_baseClient.GetUri("/openApi/user/auth/userDataStream"), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false);
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
            return await _baseClient.SendRequestInternal(_baseClient.GetUri("/openApi/user/auth/userDataStream"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Margin Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BingXMarginMode>> GetMarginModeAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            return await _baseClient.SendRequestInternal<BingXMarginMode>(_baseClient.GetUri("/openApi/swap/v2/trade/marginType"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Set Margin Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BingXMarginMode>> SetMarginModeAsync(string symbol, MarginMode marginMode, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddEnum("marginType", marginMode);
            return await _baseClient.SendRequestInternal<BingXMarginMode>(_baseClient.GetUri("/openApi/swap/v2/trade/marginType"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<BingXLeverage>> GetLeverageAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            return await _baseClient.SendRequestInternal<BingXLeverage>(_baseClient.GetUri("/openApi/swap/v2/trade/leverage"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<BingXLeverageResult>> SetLeverageAsync(string symbol, PositionSide side, int leverage, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "leverage", leverage }
            };
            parameters.AddEnum("side", side);
            return await _baseClient.SendRequestInternal<BingXLeverageResult>(_baseClient.GetUri("/openApi/swap/v2/trade/leverage"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Set Isolated Margin

        /// <inheritdoc />
        public async Task<WebCallResult<BingXIsolatedMarginResult>> AdjustIsolatedMarginAsync(string symbol, decimal quantity, AdjustDirection direction, PositionSide side, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "amount", quantity }
            };
            parameters.AddEnum("type", direction);
            parameters.AddEnum("positionSide", side);
            return await _baseClient.SendRequestInternal<BingXIsolatedMarginResult>(_baseClient.GetUri("/openApi/swap/v2/trade/positionMargin"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Position Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BingXPositionMode>> GetPositionModeAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            return await _baseClient.SendRequestInternal<BingXPositionMode>(_baseClient.GetUri("/openApi/swap/v1/positionSide/dual"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Set Position Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BingXPositionMode>> SetPositionModeAsync(string symbol, PositionMode positionMode, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddEnum("dualSidePosition", positionMode);
            return await _baseClient.SendRequestInternal<BingXPositionMode>(_baseClient.GetUri("/openApi/swap/v1/positionSide/dual"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

    }
}
