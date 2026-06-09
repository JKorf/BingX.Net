using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;
using BingX.Net.Objects.Internal;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    internal class BingXRestClientPerpetualFuturesApiAccount : IBingXRestClientPerpetualFuturesApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly BingXRestClientPerpetualFuturesApi _baseClient;

        internal BingXRestClientPerpetualFuturesApiAccount(BingXRestClientPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesBalance[]>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v3/user/balance", BingXExchange.RateLimiter.RestAccount2, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXFuturesBalance[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Incomes

        /// <inheritdoc />
        public async Task<HttpResult<BingXIncome[]>> GetIncomesAsync(string? symbol = null, IncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("incomeType", incomeType);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/user/income", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXIncome[]>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success && result.Data == null)
            {
                // No items returns null; return empty array instead
                return HttpResult.Ok<BingXIncome[]>(result, Array.Empty<BingXIncome>());
            }
            return result;
        }

        #endregion

        #region Get Trading Fees

        /// <inheritdoc />
        public async Task<HttpResult<BingXFuturesTradingFees>> GetTradingFeesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/user/commissionRate", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXFuturesTradingFeesWrapper>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BingXFuturesTradingFees>(result);

            return HttpResult.Ok(result, result.Data.Rates);
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<HttpResult<string>> StartUserStreamAsync(CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return HttpResult.Fail<string>(_baseClient.Exchange, new NoApiCredentialsError());

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/user/auth/userDataStream", BingXExchange.RateLimiter.RestAccount1, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendRawAsync<BingXListenKey>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<string>(result);

            return HttpResult.Ok(result, result.Data.ListenKey);
        }

        #endregion

        #region Keep Alive User Stream

        /// <inheritdoc />
        public async Task<HttpResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return HttpResult.Fail(_baseClient.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "listenKey", listenKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Put, _baseClient.BaseAddress, "/openApi/user/auth/userDataStream", BingXExchange.RateLimiter.RestAccount1, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey), parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Stop User Stream

        /// <inheritdoc />
        public async Task<HttpResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return HttpResult.Fail(_baseClient.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "listenKey", listenKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/openApi/user/auth/userDataStream", BingXExchange.RateLimiter.RestAccount1, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Margin Mode

        /// <inheritdoc />
        public async Task<HttpResult<BingXMarginMode>> GetMarginModeAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/trade/marginType", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXMarginMode>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Margin Mode

        /// <inheritdoc />
        public async Task<HttpResult> SetMarginModeAsync(string symbol, MarginMode marginMode, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            parameters.Add("marginType", marginMode);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/marginType", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey), parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Leverage

        /// <inheritdoc />
        public async Task<HttpResult<BingXLeverage>> GetLeverageAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v2/trade/leverage", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXLeverage>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<HttpResult<BingXLeverageResult>> SetLeverageAsync(string symbol, PositionSide side, int leverage, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol },
                { "leverage", leverage }
            };
            parameters.Add("side", side);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/leverage", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXLeverageResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Isolated Margin

        /// <inheritdoc />
        public async Task<HttpResult> AdjustIsolatedMarginAsync(string symbol, decimal quantity, AdjustDirection direction, PositionSide side, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings)
            {
                { "symbol", symbol },
                { "amount", quantity }
            };
            parameters.Add("type", direction);
            parameters.Add("positionSide", side);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v2/trade/positionMargin", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Position Mode

        /// <inheritdoc />
        public async Task<HttpResult<BingXPositionMode>> GetPositionModeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/positionSide/dual", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXPositionMode>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Position Mode

        /// <inheritdoc />
        public async Task<HttpResult<BingXPositionMode>> SetPositionModeAsync(PositionMode positionMode, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("dualSidePosition", positionMode);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v1/positionSide/dual", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<BingXPositionMode>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Isolated Margin Change History

        /// <inheritdoc />
        public async Task<HttpResult<BingXMarginHistory>> GetIsolatedMarginChangeHistoryAsync(string positionId, string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("positionId", positionId);
            parameters.Add("symbol", symbol);
            parameters.Add("startTime", startTime ?? DateTime.UtcNow.AddDays(-30));
            parameters.Add("endTime", endTime);
            parameters.Add("pageIndex", page ?? 1);
            parameters.Add("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/positionMargin/history", BingXExchange.RateLimiter.RestAccount1, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<BingXMarginHistory>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Apply For VST Assets

        /// <inheritdoc />
        public async Task<HttpResult<BingXAmount>> ApplyForVSTAssetsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v1/trade/getVst", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXAmount>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Multi Asset Mode

        /// <inheritdoc />
        public async Task<HttpResult<BingXMultiAssetMode>> SetMultiAssetModeAsync(MultiAssetMode assetMode, CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            parameters.Add("assetMode", assetMode);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/openApi/swap/v1/trade/assetMode", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXMultiAssetMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Multi Asset Mode

        /// <inheritdoc />
        public async Task<HttpResult<BingXMultiAssetMode>> GetMultiAssetModeAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/trade/assetMode", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXMultiAssetMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Multi Asset Rules

        /// <inheritdoc />
        public async Task<HttpResult<BingXMultiAssetRules[]>> GetMultiAssetRulesAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/trade/multiAssetsRules", BingXExchange.RateLimiter.RestMarket, 1, true);
            var result = await _baseClient.SendAsync<BingXMultiAssetRules[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Multi Assets Margin

        /// <inheritdoc />
        public async Task<HttpResult<BingXMarginAsset[]>> GetMultiAssetsMarginAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BingXExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/openApi/swap/v1/user/marginAssets", BingXExchange.RateLimiter.RestAccount1, 1, true, limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<BingXMarginAsset[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
