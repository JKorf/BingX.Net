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

    }
}
