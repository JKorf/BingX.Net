using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXRestClientPerpetualFuturesApi : IBingXRestClientPerpetualFuturesApiShared
    {
        private const string _exchangeName = "BingX";
        private const string _topicId = "BingXPerpFutures";

        public string Exchange => BingXExchange.ExchangeName;
        public TradingMode[] SupportedTradingModes => new[] { TradingMode.PerpetualLinear };
        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Klines client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, true, false, true, 1000, false);

        Task<ExchangeWebResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IKlineRestClient, GetKlinesRequest, SharedKline[]>(
                this,
                client => client.GetKlinesOptions,
                request,
                async () =>
                {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return SharedExecutionResult<SharedKline[]>.Error(new ExchangeWebResult<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported")));

            // Descending order doesn't work; too little klines are returned when specifying only end time
            var direction = DataDirection.Ascending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 1000;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                pageParams.StartTime,
                pageParams.EndTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedKline[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data.Max(x => x.Timestamp)),
                    result.Data.Length,
                    result.Data.Select(x => x.Timestamp),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            // Return
            return SharedExecutionResult<SharedKline[]>.Ok(result,
                ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedKline(request.Symbol, symbol, x.Timestamp, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume))
                    .ToArray(), nextPageRequest);
        
                });
        }

        #endregion

        #region Futures Symbol client

        EndpointOptions<GetSymbolsRequest, IFuturesSymbolRestClient> IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest, IFuturesSymbolRestClient>(_exchangeName, false);
        Task<ExchangeWebResult<SharedFuturesSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesSymbolRestClient, GetSymbolsRequest, SharedFuturesSymbol[]>(
                this,
                client => client.GetFuturesSymbolsOptions,
                request,
                async () =>
                {

            var result = await ExchangeData.GetContractsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedFuturesSymbol[]>.Error(result);

            var resultData = result.Data.Where(x => !string.IsNullOrEmpty(x.Asset)).Select(s => new SharedFuturesSymbol(TradingMode.PerpetualLinear, s.Asset, s.Currency, s.Symbol, s.Status == 1)
            {
                MinTradeQuantity = s.MinOrderQuantity,
                MinNotionalValue = s.MinOrderValue,
                PriceDecimals = s.PricePrecision,
                QuantityDecimals = s.QuantityPrecision,
                ContractSize = 1,
                MaxShortLeverage = s.MaxShortLeverage,
                MaxLongLeverage = s.MaxLongLeverage
            }).ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, resultData);
            return SharedExecutionResult<SharedFuturesSymbol[]>.Ok(result, resultData);
        
                });
        }

        async Task<ExchangeResult<SharedSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<SharedSymbol[]>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<SharedSymbol[]>(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, baseAsset));
        }

        async Task<ExchangeResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbol));
        }

        async Task<ExchangeResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbolName));
        }
        #endregion

        #region Futures Ticker client

        GetFuturesTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new GetFuturesTickerOptions(_exchangeName);
        Task<ExchangeWebResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesTickerRestClient, GetTickerRequest, SharedFuturesTicker>(
                this,
                client => client.GetFuturesTickerOptions,
                request,
                async () =>
                {

            var resultTicker = ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct);
            var resultFunding = ExchangeData.GetFundingRateAsync(request.Symbol.GetSymbol(FormatSymbol), ct);
            await Task.WhenAll(resultTicker, resultFunding).ConfigureAwait(false);
            if (!resultTicker.Result)
                return SharedExecutionResult<SharedFuturesTicker>.Error(resultTicker.Result);
            if (!resultFunding.Result)
                return SharedExecutionResult<SharedFuturesTicker>.Error(resultFunding.Result);

            return SharedExecutionResult<SharedFuturesTicker>.Ok(resultTicker.Result, new SharedFuturesTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, resultTicker.Result.Data.Symbol),
                    resultTicker.Result.Data.Symbol,
                    resultTicker.Result.Data.LastPrice,
                    resultTicker.Result.Data.HighPrice,
                    resultTicker.Result.Data.LowPrice,
                    resultTicker.Result.Data.Volume,
                    resultTicker.Result.Data.PriceChangePercent)
                {
                    IndexPrice = resultFunding.Result.Data.IndexPrice,
                    MarkPrice = resultFunding.Result.Data.MarkPrice,
                    FundingRate = resultFunding.Result.Data.LastFundingRate,
                    NextFundingTime = resultFunding.Result.Data.NextFundingTime
                });
        
                });
        }

        GetFuturesTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new GetFuturesTickersOptions(_exchangeName);
        Task<ExchangeWebResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesTickerRestClient, GetTickersRequest, SharedFuturesTicker[]>(
                this,
                client => client.GetFuturesTickersOptions,
                request,
                async () =>
                {

            var resultTickers = ExchangeData.GetTickersAsync(ct: ct);
            var resultFunding = ExchangeData.GetFundingRatesAsync(ct: ct);
            await Task.WhenAll(resultTickers, resultFunding).ConfigureAwait(false);
            if (!resultTickers.Result)
                return SharedExecutionResult<SharedFuturesTicker[]>.Error(resultTickers.Result);
            if (!resultFunding.Result)
                return SharedExecutionResult<SharedFuturesTicker[]>.Error(resultFunding.Result);

            return SharedExecutionResult<SharedFuturesTicker[]>.Ok(resultTickers.Result, resultTickers.Result.Data.Select(x =>
            {
                var markPrice = resultFunding.Result.Data.Single(p => p.Symbol == x.Symbol);
                return new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, x.PriceChangePercent)
                {
                    MarkPrice = markPrice.MarkPrice,
                    IndexPrice = markPrice.IndexPrice,
                    FundingRate = markPrice.LastFundingRate,
                    NextFundingTime = markPrice.NextFundingTime
                };
            }).ToArray());
        
                });
        }

        #endregion

        #region Book Ticker client

        EndpointOptions<GetBookTickerRequest, IBookTickerRestClient> IBookTickerRestClient.GetBookTickerOptions { get; } = new EndpointOptions<GetBookTickerRequest, IBookTickerRestClient>(_exchangeName, false);
        Task<ExchangeWebResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IBookTickerRestClient, GetBookTickerRequest, SharedBookTicker>(
                this,
                client => client.GetBookTickerOptions,
                request,
                async () =>
                {

            var resultTicker = await ExchangeData.GetBookTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return SharedExecutionResult<SharedBookTicker>.Error(resultTicker);

            return SharedExecutionResult<SharedBookTicker>.Ok(resultTicker, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, resultTicker.Data.Symbol),
                resultTicker.Data.Symbol,
                resultTicker.Data.BestAskPrice,
                resultTicker.Data.BestAskQuantity,
                resultTicker.Data.BestBidPrice,
                resultTicker.Data.BestBidQuantity));
        
                });
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 1000, false);
        Task<ExchangeWebResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IRecentTradeRestClient, GetRecentTradesRequest, SharedTrade[]>(
                this,
                client => client.GetRecentTradesOptions,
                request,
                async () =>
                {

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetRecentTradesAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedTrade[]>.Error(result);

            return SharedExecutionResult<SharedTrade[]>.Ok(result, result.Data.AsEnumerable().Reverse().Select(x => 
                new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.BuyerIsMaker ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray());
        
                });
        }

        #endregion

        #region Leverage client
        SharedLeverageSettingMode ILeverageRestClient.LeverageSettingType => SharedLeverageSettingMode.PerSide;

        EndpointOptions<GetLeverageRequest, ILeverageRestClient> ILeverageRestClient.GetLeverageOptions { get; } = new EndpointOptions<GetLeverageRequest, ILeverageRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ILeverageRestClient, GetLeverageRequest, SharedLeverage>(
                this,
                client => client.GetLeverageOptions,
                request,
                async () =>
                {

            var result = await Account.GetLeverageAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedLeverage>.Error(result);

            return SharedExecutionResult<SharedLeverage>.Ok(result, new SharedLeverage(request.PositionSide == SharedPositionSide.Short ? result.Data.ShortLeverage : result.Data.LongLeverage) { 
                Side = request.PositionSide
            });
        
                });
        }

        SetLeverageOptions ILeverageRestClient.SetLeverageOptions { get; } = new SetLeverageOptions(_exchangeName);
        Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ILeverageRestClient, SetLeverageRequest, SharedLeverage>(
                this,
                client => client.SetLeverageOptions,
                request,
                async () =>
                {

            var result = await Account.SetLeverageAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == null ? PositionSide.Both : request.Side == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                (int)request.Leverage,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedLeverage>.Error(result);

            return SharedExecutionResult<SharedLeverage>.Ok(result, new SharedLeverage(result.Data.Leverage)
            {
                Side = request.Side
            });
        
                });
        }
        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, new[] { 5, 10, 20, 50, 100, 500, 1000 }, false);
        Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IOrderBookRestClient, GetOrderBookRequest, SharedOrderBook>(
                this,
                client => client.GetOrderBookOptions,
                request,
                async () =>
                {

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedOrderBook>.Error(result);

            return SharedExecutionResult<SharedOrderBook>.Ok(result, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        
                });
        }

        #endregion

        #region Index Klines client

        GetKlinesOptions IIndexPriceKlineRestClient.GetIndexPriceKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 1440, false);

        Task<ExchangeWebResult<SharedFuturesKline[]>> IIndexPriceKlineRestClient.GetIndexPriceKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IKlineRestClient, GetKlinesRequest, SharedFuturesKline[]>(
                this,
                client => ((IIndexPriceKlineRestClient)this).GetIndexPriceKlinesOptions,
                request,
                async () =>
                {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return SharedExecutionResult<SharedFuturesKline[]>.Error(new ExchangeWebResult<SharedFuturesKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported")));


            var direction = DataDirection.Ascending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 1000;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            var result = await ExchangeData.GetMarkPriceKlinesAsync(
                symbol,
                interval,
                pageParams.StartTime,
                pageParams.EndTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedFuturesKline[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data.Max(x => x.OpenTime)),
                    result.Data.Length,
                    result.Data.Select(x => x.OpenTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            // Return
            return SharedExecutionResult<SharedFuturesKline[]>.Ok(result,
                ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedFuturesKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice))
                    .ToArray(), nextPageRequest);
        
                });
        }

        #endregion

        #region Mark Klines client

        GetKlinesOptions IMarkPriceKlineRestClient.GetMarkPriceKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 1440, false);

        Task<ExchangeWebResult<SharedFuturesKline[]>> IMarkPriceKlineRestClient.GetMarkPriceKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IKlineRestClient, GetKlinesRequest, SharedFuturesKline[]>(
                this,
                client => ((IMarkPriceKlineRestClient)this).GetMarkPriceKlinesOptions,
                request,
                async () =>
                {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return SharedExecutionResult<SharedFuturesKline[]>.Error(new ExchangeWebResult<SharedFuturesKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported")));

            var direction = DataDirection.Ascending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 1000;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            var result = await ExchangeData.GetMarkPriceKlinesAsync(
                symbol,
                interval,
                pageParams.StartTime,
                pageParams.EndTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedFuturesKline[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data.Max(x => x.OpenTime)),
                    result.Data.Length,
                    result.Data.Select(x => x.OpenTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            // Return
            return SharedExecutionResult<SharedFuturesKline[]>.Ok(result,
                ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedFuturesKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice))
                    .ToArray(), nextPageRequest);
        
                });
        }

        #endregion

        #region Open Interest client

        EndpointOptions<GetOpenInterestRequest, IOpenInterestRestClient> IOpenInterestRestClient.GetOpenInterestOptions { get; } = new EndpointOptions<GetOpenInterestRequest, IOpenInterestRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedOpenInterest>> IOpenInterestRestClient.GetOpenInterestAsync(GetOpenInterestRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IOpenInterestRestClient, GetOpenInterestRequest, SharedOpenInterest>(
                this,
                client => client.GetOpenInterestOptions,
                request,
                async () =>
                {

            var result = await ExchangeData.GetOpenInterestAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedOpenInterest>.Error(result);

            return SharedExecutionResult<SharedOpenInterest>.Ok(result, new SharedOpenInterest(result.Data.OpenInterest));
        
                });
        }

        #endregion

        #region Funding Rate client
        GetFundingRateHistoryOptions IFundingRateRestClient.GetFundingRateHistoryOptions { get; } = new GetFundingRateHistoryOptions(_exchangeName, false, true, true, 1000, false);

        Task<ExchangeWebResult<SharedFundingRate[]>> IFundingRateRestClient.GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFundingRateRestClient, GetFundingRateHistoryRequest, SharedFundingRate[]>(
                this,
                client => client.GetFundingRateHistoryOptions,
                request,
                async () =>
                {

            var direction = DataDirection.Descending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 1000;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, true);

            // Get data
            var result = await ExchangeData.GetFundingRateHistoryAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.StartTime ?? pageParams.EndTime?.AddHours(-(8 * pageParams.Limit)),
                endTime: pageParams.EndTime,
                limit: limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedFundingRate[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.FundingTime)),
                    result.Data.Length,
                    result.Data.Select(x => x.FundingTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            // Return
            return SharedExecutionResult<SharedFundingRate[]>.Ok(result,
                ExchangeHelpers.ApplyFilter(result.Data, x => x.FundingTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedFundingRate(x.FundingRate, x.FundingTime))
                    .ToArray(), nextPageRequest);
        
                });
        }
        #endregion

        #region Futures Order Client
        
        SharedFeeDeductionType IFuturesOrderRestClient.FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType IFuturesOrderRestClient.FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        SharedOrderType[] IFuturesOrderRestClient.FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        SharedTimeInForce[] IFuturesOrderRestClient.FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport IFuturesOrderRestClient.FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset);

        string IFuturesOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(40).ToLowerInvariant();

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, PlaceFuturesOrderRequest, SharedId>(
                this,
                client => client.PlaceFuturesOrderOptions,
                request,
                async () =>
                {

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.FuturesOrderType.Limit : Enums.FuturesOrderType.Market,
                quantity: request.Quantity?.QuantityInBaseAsset ?? request.Quantity?.QuantityInContracts,
                price: request.Price,
                positionSide: request.PositionSide == null ? PositionSide.Both : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                reduceOnly: request.ReduceOnly,
                timeInForce: GetTimeInForce(request.TimeInForce),
                clientOrderId: request.ClientOrderId,

                takeProfitStopPrice: request.TakeProfitPrice,
                takeProfitType: request.TakeProfitPrice == null ? null : TakeProfitStopLossMode.TakeProfitMarket,
                stopLossStopPrice: request.StopLossPrice,
                stopLossType: request.StopLossPrice == null ? null : TakeProfitStopLossMode.StopMarket,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return SharedExecutionResult<SharedId>.Error(result);

            return SharedExecutionResult<SharedId>.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        
                });
        }

        EndpointOptions<GetOrderRequest, IFuturesOrderRestClient> IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new EndpointOptions<GetOrderRequest, IFuturesOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, GetOrderRequest, SharedFuturesOrder>(
                this,
                client => client.GetFuturesOrderOptions,
                request,
                async () =>
                {

            if (!long.TryParse(request.OrderId, out var orderId))
                return SharedExecutionResult<SharedFuturesOrder>.Error(new ExchangeWebResult<SharedFuturesOrder>(Exchange, ArgumentError.Invalid(nameof(GetOrderRequest.OrderId), "Invalid order id")));

            var order = await Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return SharedExecutionResult<SharedFuturesOrder>.Error(order);

            return SharedExecutionResult<SharedFuturesOrder>.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId.ToString(),
                ParseOrderType(order.Data.Type),
                order.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                AveragePrice = order.Data.AveragePrice == 0 ? null : order.Data.AveragePrice,
                OrderPrice = order.Data.Price,
                Leverage = order.Data.Leverage?.EndsWith("X") == true ? decimal.Parse(order.Data.Leverage.Substring(0, order.Data.Leverage.Length - 1)) : null,
                OrderQuantity = new SharedOrderQuantity(order.Data.Quantity, null, order.Data.Quantity),
                QuantityFilled = new SharedOrderQuantity(order.Data.QuantityFilled, order.Data.ValueFilled, order.Data.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.Data.TimeInForce),
                UpdateTime = order.Data.UpdateTime,
                PositionSide = order.Data.PositionSide == PositionSide.Both ? null : order.Data.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                ReduceOnly = order.Data.ReduceOnly,
                TakeProfitPrice = order.Data.TakeProfit?.StopPrice == 0 ? null : order.Data.TakeProfit?.StopPrice,
                StopLossPrice = order.Data.StopLoss?.StopPrice == 0 ? null : order.Data.StopLoss?.StopPrice,
                TriggerPrice = order.Data.StopPrice,
                IsTriggerOrder = order.Data.StopPrice > 0,
                IsCloseOrder = order.Data.ClosePosition
            });
        
                });
        }

        EndpointOptions<GetOpenOrdersRequest, IFuturesOrderRestClient> IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest, IFuturesOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, GetOpenOrdersRequest, SharedFuturesOrder[]>(
                this,
                client => client.GetOpenFuturesOrdersOptions,
                request,
                async () =>
                {

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!orders)
                return SharedExecutionResult<SharedFuturesOrder[]>.Error(orders);

            return SharedExecutionResult<SharedFuturesOrder[]>.Ok(orders, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), 
                x.Symbol,
                x.OrderId.ToString(),
                ParseOrderType(x.Type),
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                OrderPrice = x.Price,
                OrderQuantity = new SharedOrderQuantity(x.Quantity, null, x.Quantity),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.ValueFilled, x.QuantityFilled),
                Leverage = x.Leverage?.EndsWith("X") == true ? decimal.Parse(x.Leverage.Substring(0, x.Leverage.Length - 1)) : null,
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.UpdateTime,
                PositionSide = x.PositionSide == PositionSide.Both ? null : x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                ReduceOnly = x.ReduceOnly,
                TakeProfitPrice = x.TakeProfit?.StopPrice == 0 ? null: x.TakeProfit?.StopPrice,
                StopLossPrice = x.StopLoss?.StopPrice == 0 ? null : x.StopLoss?.StopPrice,
                TriggerPrice = x.StopPrice,
                IsTriggerOrder = x.StopPrice > 0,
                IsCloseOrder = x.ClosePosition
            }).ToArray());
        
                });
        }

        GetFuturesClosedOrdersOptions IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, true, true, true, 1000);
        Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, GetClosedOrdersRequest, SharedFuturesOrder[]>(
                this,
                client => client.GetClosedFuturesOrdersOptions,
                request,
                async () =>
                {

            var direction = request.Direction ?? DataDirection.Ascending;
            var limit = request.Limit ?? 1000;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, maxPeriod: TimeSpan.FromDays(7));

            var result = await Trading.GetClosedOrdersAsync(
                symbol,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: limit,
                orderId: pageParams.FromId == null ? null : long.Parse(pageParams.FromId),
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedFuturesOrder[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                   () => direction == DataDirection.Ascending
                       ? Pagination.NextPageFromId(result.Data.Max(x => x.OrderId) + 1)
                       : Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.CreateTime)),
                   result.Data.Length,
                   result.Data.Select(x => x.CreateTime),
                   request.StartTime,
                   request.EndTime ?? DateTime.UtcNow,
                   pageParams,
                   TimeSpan.FromDays(7));

            return SharedExecutionResult<SharedFuturesOrder[]>.Ok(result,
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedFuturesOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), 
                            x.Symbol,
                            x.OrderId.ToString(),
                            ParseOrderType(x.Type),
                            x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.CreateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            AveragePrice = x.AveragePrice,
                            OrderPrice = x.Price,
                            OrderQuantity = new SharedOrderQuantity(x.Quantity, null, x.Quantity),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.ValueFilled, x.QuantityFilled),
                            Leverage = x.Leverage?.EndsWith("X") == true ? decimal.Parse(x.Leverage.Substring(0, x.Leverage.Length - 1)) : null,
                            TimeInForce = ParseTimeInForce(x.TimeInForce),
                            UpdateTime = x.UpdateTime,
                            PositionSide = x.PositionSide == PositionSide.Both ? null : x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                            ReduceOnly = x.ReduceOnly,
                            TakeProfitPrice = x.TakeProfit?.StopPrice == 0 ? null : x.TakeProfit?.StopPrice,
                            StopLossPrice = x.StopLoss?.StopPrice == 0 ? null : x.StopLoss?.StopPrice,
                            TriggerPrice = x.StopPrice,
                            IsTriggerOrder = x.StopPrice > 0,
                            IsCloseOrder = x.ClosePosition
                        })
                    .ToArray(), nextPageRequest);
        
                });
        }

        EndpointOptions<GetOrderTradesRequest, IFuturesOrderRestClient> IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest, IFuturesOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, GetOrderTradesRequest, SharedUserTrade[]>(
                this,
                client => client.GetFuturesOrderTradesOptions,
                request,
                async () =>
                {

            if (!long.TryParse(request.OrderId, out var orderId))
                return SharedExecutionResult<SharedUserTrade[]>.Error(new ExchangeWebResult<SharedUserTrade[]>(Exchange, ArgumentError.Invalid(nameof(GetOrderTradesRequest.OrderId), "Invalid order id")));

            var orders = await Trading.GetUserTradesAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId: orderId, ct: ct).ConfigureAwait(false);
            if (!orders)
                return SharedExecutionResult<SharedUserTrade[]>.Error(orders);

            return SharedExecutionResult<SharedUserTrade[]>.Ok(orders, orders.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), 
                x.Symbol,
                x.OrderId.ToString(),
                x.TradeId,
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Fee = Math.Abs(x.Fee),
                FeeAsset = x.FeeAsset,
                Role = x.Role == Role.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        
                });
        }

        GetFuturesUserTradesOptions IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new GetFuturesUserTradesOptions(_exchangeName, true, true, true, 1000);
        Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, GetUserTradesRequest, SharedUserTrade[]>(
                this,
                client => client.GetFuturesUserTradesOptions,
                request,
                async () =>
                {

            var direction = request.Direction ?? DataDirection.Ascending;
            var limit = request.Limit ?? 1000;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, maxPeriod: TimeSpan.FromDays(7));

            // Get data
            var result = await Trading.GetUserTradesAsync(
                symbol,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: limit,
                fromId: pageParams.FromId == null ? null : long.Parse(pageParams.FromId),
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedUserTrade[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                   () => direction == DataDirection.Ascending
                       ? Pagination.NextPageFromId(result.Data.Max(x => x.TradeId) + 1)
                       : Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.Timestamp)),
                   result.Data.Length,
                   result.Data.Select(x => x.Timestamp),
                   request.StartTime,
                   request.EndTime ?? DateTime.UtcNow,
                   pageParams,
                   TimeSpan.FromDays(7));

            return SharedExecutionResult<SharedUserTrade[]>.Ok(result,
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), 
                            x.Symbol,
                            x.OrderId.ToString(),
                            x.TradeId.ToString(),
                            x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.Quantity,
                            x.Price,
                            x.Timestamp)
                        {
                            Fee = Math.Abs(x.Fee),
                            FeeAsset = x.FeeAsset,
                            Role = x.Role == Role.Maker ? SharedRole.Maker : SharedRole.Taker
                        }).ToArray(), nextPageRequest);
        
                });
        }

        EndpointOptions<CancelOrderRequest, IFuturesOrderRestClient> IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new EndpointOptions<CancelOrderRequest, IFuturesOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, CancelOrderRequest, SharedId>(
                this,
                client => client.CancelFuturesOrderOptions,
                request,
                async () =>
                {

            if (!long.TryParse(request.OrderId, out var orderId))
                return SharedExecutionResult<SharedId>.Error(new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id")));

            var order = await Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return SharedExecutionResult<SharedId>.Error(order);

            return SharedExecutionResult<SharedId>.Ok(order, new SharedId(order.Data.OrderId.ToString()));
        
                });
        }

        EndpointOptions<GetPositionsRequest, IFuturesOrderRestClient> IFuturesOrderRestClient.GetPositionsOptions { get; } = new EndpointOptions<GetPositionsRequest, IFuturesOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedPosition[]>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, GetPositionsRequest, SharedPosition[]>(
                this,
                client => client.GetPositionsOptions,
                request,
                async () =>
                {

            var result = await Trading.GetPositionsAsync(symbol: request.Symbol?.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedPosition[]>.Error(result);

            return SharedExecutionResult<SharedPosition[]>.Ok(result, result.Data.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, x.Size, x.UpdateTime)
            {
                UnrealizedPnl = x.UnrealizedProfit,
                LiquidationPrice = x.LiquidationPrice,
                Leverage = x.Leverage,
                AverageOpenPrice = x.AveragePrice,
                PositionMode = SharedPositionMode.HedgeMode,
                PositionSide = x.Side == TradeSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long
            }).ToArray());
        
                });
        }

        EndpointOptions<ClosePositionRequest, IFuturesOrderRestClient> IFuturesOrderRestClient.ClosePositionOptions { get; } = new EndpointOptions<ClosePositionRequest, IFuturesOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderRestClient, ClosePositionRequest, SharedId>(
                this,
                client => client.ClosePositionOptions,
                request,
                async () =>
                {

            var positions = await Trading.GetPositionsAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!positions)
                return SharedExecutionResult<SharedId>.Error(positions);

            var position = positions.Data.FirstOrDefault(x => request.PositionSide == null ? x.Size != 0 : x.Side == (request.PositionSide == SharedPositionSide.Short ? TradeSide.Short : TradeSide.Long));
            if (position == null)
                return SharedExecutionResult<SharedId>.Error(new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var result = await Trading.ClosePositionAsync(position.PositionId).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedId>.Error(result);

            return SharedExecutionResult<SharedId>.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        
                });
        }

        private TimeInForce? GetTimeInForce(SharedTimeInForce? tif)
        {
            if (tif == null)
                return null;

            if (tif == SharedTimeInForce.ImmediateOrCancel) return TimeInForce.ImmediateOrCancel;
            if (tif == SharedTimeInForce.FillOrKill) return TimeInForce.FillOrKill;
            if (tif == SharedTimeInForce.GoodTillCanceled) return TimeInForce.GoodTillCanceled;

            return null;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == Enums.OrderStatus.Canceled || status == OrderStatus.Failed)
                return SharedOrderStatus.Canceled;
            if (status == Enums.OrderStatus.New || status == Enums.OrderStatus.Pending || status == Enums.OrderStatus.PartiallyFilled || status == OrderStatus.Working)
                return SharedOrderStatus.Open;
            if (status == OrderStatus.Filled)
                return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(FuturesOrderType type)
        {
            if (type == FuturesOrderType.Market) return SharedOrderType.Market;
            if (type == FuturesOrderType.Limit) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private SharedTimeInForce? ParseTimeInForce(TimeInForce? tif)
        {
            if (tif == null)
                return null;

            if (tif == TimeInForce.GoodTillCanceled) return SharedTimeInForce.GoodTillCanceled;
            if (tif == TimeInForce.ImmediateOrCancel) return SharedTimeInForce.ImmediateOrCancel;
            if (tif == TimeInForce.FillOrKill) return SharedTimeInForce.FillOrKill;

            return null;
        }

        #endregion

        #region Futures Client Id Order Client

        EndpointOptions<GetOrderRequest, IFuturesOrderClientIdRestClient> IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdOptions { get; } = new EndpointOptions<GetOrderRequest, IFuturesOrderClientIdRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedFuturesOrder>> IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderClientIdRestClient, GetOrderRequest, SharedFuturesOrder>(
                this,
                client => client.GetFuturesOrderByClientOrderIdOptions,
                request,
                async () =>
                {

            var order = await Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return SharedExecutionResult<SharedFuturesOrder>.Error(order);

            return SharedExecutionResult<SharedFuturesOrder>.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId.ToString(),
                ParseOrderType(order.Data.Type),
                order.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                AveragePrice = order.Data.AveragePrice == 0 ? null : order.Data.AveragePrice,
                OrderPrice = order.Data.Price,
                Leverage = order.Data.Leverage?.EndsWith("X") == true ? decimal.Parse(order.Data.Leverage.Substring(0, order.Data.Leverage.Length - 1)) : null,
                OrderQuantity = new SharedOrderQuantity(order.Data.Quantity, null, order.Data.Quantity),
                QuantityFilled = new SharedOrderQuantity(order.Data.QuantityFilled, order.Data.ValueFilled, order.Data.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.Data.TimeInForce),
                UpdateTime = order.Data.UpdateTime,
                PositionSide = order.Data.PositionSide == PositionSide.Both ? null : order.Data.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                ReduceOnly = order.Data.ReduceOnly,
                TakeProfitPrice = order.Data.TakeProfit?.StopPrice == 0 ? null : order.Data.TakeProfit?.StopPrice,
                StopLossPrice = order.Data.StopLoss?.StopPrice == 0 ? null : order.Data.StopLoss?.StopPrice,
                TriggerPrice = order.Data.StopPrice,
                IsTriggerOrder = order.Data.StopPrice > 0,
                IsCloseOrder = order.Data.ClosePosition
            });
        
                });
        }

        EndpointOptions<CancelOrderRequest, IFuturesOrderClientIdRestClient> IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdOptions { get; } = new EndpointOptions<CancelOrderRequest, IFuturesOrderClientIdRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesOrderClientIdRestClient, CancelOrderRequest, SharedId>(
                this,
                client => client.CancelFuturesOrderByClientOrderIdOptions,
                request,
                async () =>
                {

            var order = await Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return SharedExecutionResult<SharedId>.Error(order);

            return SharedExecutionResult<SharedId>.Ok(order, new SharedId(order.Data.OrderId.ToString()));
        
                });
        }
        #endregion

        #region Balance client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Futures);

        Task<ExchangeWebResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IBalanceRestClient, GetBalancesRequest, SharedBalance[]>(
                this,
                client => client.GetBalancesOptions,
                request,
                async () =>
                {

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedBalance[]>.Error(result);

            return SharedExecutionResult<SharedBalance[]>.Ok(result, result.Data.Where(x => !string.IsNullOrEmpty(x.Asset)).Select(x => 
                new SharedBalance(x.Asset, x.AvailableMargin ?? 0, x.Equity ?? 0)).ToArray());
        
                });
        }

        #endregion

        #region Position Mode client

        SharedPositionModeSelection IPositionModeRestClient.PositionModeSettingType => SharedPositionModeSelection.PerAccount;

        GetPositionModeOptions IPositionModeRestClient.GetPositionModeOptions { get; } = new GetPositionModeOptions(_exchangeName);
        Task<ExchangeWebResult<SharedPositionModeResult>> IPositionModeRestClient.GetPositionModeAsync(GetPositionModeRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IPositionModeRestClient, GetPositionModeRequest, SharedPositionModeResult>(
                this,
                client => client.GetPositionModeOptions,
                request,
                async () =>
                {

            var result = await Account.GetPositionModeAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedPositionModeResult>.Error(result);

            return SharedExecutionResult<SharedPositionModeResult>.Ok(result, new SharedPositionModeResult(result.Data.PositionMode == PositionMode.DualPositionMode ? SharedPositionMode.HedgeMode : SharedPositionMode.OneWay));
        
                });
        }

        SetPositionModeOptions IPositionModeRestClient.SetPositionModeOptions { get; } = new SetPositionModeOptions(_exchangeName);
        Task<ExchangeWebResult<SharedPositionModeResult>> IPositionModeRestClient.SetPositionModeAsync(SetPositionModeRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IPositionModeRestClient, SetPositionModeRequest, SharedPositionModeResult>(
                this,
                client => client.SetPositionModeOptions,
                request,
                async () =>
                {

            var result = await Account.SetPositionModeAsync(request.PositionMode == SharedPositionMode.HedgeMode ? PositionMode.DualPositionMode : PositionMode.SinglePositionMode, ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedPositionModeResult>.Error(result);

            return SharedExecutionResult<SharedPositionModeResult>.Ok(result, new SharedPositionModeResult(request.PositionMode));
        
                });
        }
        #endregion

        #region Position History client

        GetPositionHistoryOptions IPositionHistoryRestClient.GetPositionHistoryOptions { get; } = new GetPositionHistoryOptions(_exchangeName, false, true, true, 1000)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetPositionHistoryRequest.Symbol), typeof(SharedSymbol), "The symbol to get position history for", "ETH-USDT")
            }
        };
        Task<ExchangeWebResult<SharedPositionHistory[]>> IPositionHistoryRestClient.GetPositionHistoryAsync(GetPositionHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IPositionHistoryRestClient, GetPositionHistoryRequest, SharedPositionHistory[]>(
                this,
                client => client.GetPositionHistoryOptions,
                request,
                async () =>
                {

            var direction = DataDirection.Descending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 1000;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetPositionHistoryAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                page: pageParams.Page,
                pageSize: limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedPositionHistory[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromPage(pageParams),
                    result.Data.Length,
                    result.Data.Select(x => x.OpenTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            return SharedExecutionResult<SharedPositionHistory[]>.Ok(result,
                ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedPositionHistory(
                            ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol),
                            x.Symbol,
                            x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                            x.AveragePrice,
                            x.AverageClosePrice,
                            x.ClosePositionQuantity,
                            x.RealisedPnl,
                            x.UpdateTime)
                        {
                            Leverage = x.Leverage,
                            PositionId = x.PositionId
                        })
                    .ToArray(), nextPageRequest);
        
                });
        }
        #endregion

        #region Listen Key client

        EndpointOptions<StartListenKeyRequest, IListenKeyRestClient> IListenKeyRestClient.StartOptions { get; } = new EndpointOptions<StartListenKeyRequest, IListenKeyRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<string>> IListenKeyRestClient.StartListenKeyAsync(StartListenKeyRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IListenKeyRestClient, StartListenKeyRequest, string>(
                this,
                client => client.StartOptions,
                request,
                async () =>
                {

            // Get data
            var result = await Account.StartUserStreamAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<string>.Error(result);

            return SharedExecutionResult<string>.Ok(result, result.Data);
        
                });
        }
        EndpointOptions<KeepAliveListenKeyRequest, IListenKeyRestClient> IListenKeyRestClient.KeepAliveOptions { get; } = new EndpointOptions<KeepAliveListenKeyRequest, IListenKeyRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<string>> IListenKeyRestClient.KeepAliveListenKeyAsync(KeepAliveListenKeyRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IListenKeyRestClient, KeepAliveListenKeyRequest, string>(
                this,
                client => client.KeepAliveOptions,
                request,
                async () =>
                {

            // Get data
            var result = await Account.KeepAliveUserStreamAsync(request.ListenKey, ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<string>.Error(result);

            return SharedExecutionResult<string>.Ok(result, request.ListenKey);
        
                });
        }

        EndpointOptions<StopListenKeyRequest, IListenKeyRestClient> IListenKeyRestClient.StopOptions { get; } = new EndpointOptions<StopListenKeyRequest, IListenKeyRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<string>> IListenKeyRestClient.StopListenKeyAsync(StopListenKeyRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IListenKeyRestClient, StopListenKeyRequest, string>(
                this,
                client => client.StopOptions,
                request,
                async () =>
                {

            // Get data
            var result = await Account.StopUserStreamAsync(request.ListenKey, ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<string>.Error(result);

            return SharedExecutionResult<string>.Ok(result, request.ListenKey);
        
                });
        }
        #endregion

        #region Fee Client
        EndpointOptions<GetFeeRequest, IFeeRestClient> IFeeRestClient.GetFeeOptions { get; } = new EndpointOptions<GetFeeRequest, IFeeRestClient>(_exchangeName, true);

        Task<ExchangeWebResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFeeRestClient, GetFeeRequest, SharedFee>(
                this,
                client => client.GetFeeOptions,
                request,
                async () =>
                {

            // Get data
            var result = await Account.GetTradingFeesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedFee>.Error(result);

            // Return
            return SharedExecutionResult<SharedFee>.Ok(result, new SharedFee(result.Data.MakerFeeRate * 100, result.Data.TakerFeeRate * 100));
        
                });
        }
        #endregion

        #region Tp/SL Client
        EndpointOptions<SetTpSlRequest, IFuturesTpSlRestClient> IFuturesTpSlRestClient.SetFuturesTpSlOptions { get; } = new EndpointOptions<SetTpSlRequest, IFuturesTpSlRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> IFuturesTpSlRestClient.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesTpSlRestClient, SetTpSlRequest, SharedId>(
                this,
                client => client.SetFuturesTpSlOptions,
                request,
                async () =>
                {

            WebCallResult<BingXFuturesOrder> result;
            if (request.TpSlSide == SharedTpSlSide.TakeProfit)
            {
                result = await Trading.PlaceOrderAsync(
                    request.Symbol!.GetSymbol(FormatSymbol),
                    OrderSide.Buy,
                    FuturesOrderType.TakeProfitMarket,
                    request.PositionMode == SharedPositionMode.OneWay ? PositionSide.Both : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                    stopPrice: request.TriggerPrice,
                    //reduceOnly: request.PositionMode == SharedPositionMode.OneWay ? true : null,
                    closePosition: true,
                    ct: ct).ConfigureAwait(false);
            }
            else
            {
                result = await Trading.PlaceOrderAsync(
                    request.Symbol!.GetSymbol(FormatSymbol),
                    OrderSide.Sell,
                    FuturesOrderType.StopMarket,
                    request.PositionMode == SharedPositionMode.OneWay ? PositionSide.Both : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                    stopPrice: request.TriggerPrice,
                    //reduceOnly: request.PositionMode == SharedPositionMode.OneWay ? true : null,
                    closePosition: true,
                    ct: ct).ConfigureAwait(false);
            }

            if (!result)
                return SharedExecutionResult<SharedId>.Error(result);

            // Return
            return SharedExecutionResult<SharedId>.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        
                });
        }

        EndpointOptions<CancelTpSlRequest, IFuturesTpSlRestClient> IFuturesTpSlRestClient.CancelFuturesTpSlOptions { get; } = new EndpointOptions<CancelTpSlRequest, IFuturesTpSlRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<bool>> IFuturesTpSlRestClient.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IFuturesTpSlRestClient, CancelTpSlRequest, bool>(
                this,
                client => client.CancelFuturesTpSlOptions,
                request,
                async () =>
                {

            if (!long.TryParse(request.OrderId, out var id))
                throw new ArgumentException($"Invalid order id");

            var result = await Trading.CancelOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                id,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<bool>.Error(result);

            // Return
            return SharedExecutionResult<bool>.Ok(result, true);
        
                });
        }

        #endregion

        private DateTime RoundDown(DateTime dt, TimeSpan d)
        {
            return new DateTime(dt.Ticks - (dt.Ticks % d.Ticks), dt.Kind);
        }
    }
}
