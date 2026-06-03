using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXRestClientSpotApi : IBingXRestClientSpotApiShared
    {
        private const string _exchangeName = "BingX";
        private const string _topicId = "BingXSpot";

        public string Exchange => BingXExchange.ExchangeName;
        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };
        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Kline client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, true, true, true, 1000, false);

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

            var direction = request.Direction ?? DataDirection.Ascending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 1000;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            // Get data
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
                    () => direction == DataDirection.Ascending
                        ? Pagination.NextPageFromTime(pageParams, result.Data.Max(x => x.OpenTime))
                        : Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.OpenTime)),
                    result.Data.Length,
                    result.Data.Select(x => x.OpenTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            // Return
            return SharedExecutionResult<SharedKline[]>.Ok(result,
                ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume))
                    .ToArray(), nextPageRequest);
        
                });
        }

        #endregion

        #region Spot Symbol client

        EndpointOptions<GetSymbolsRequest, ISpotSymbolRestClient> ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest, ISpotSymbolRestClient>(_exchangeName, false);
        Task<ExchangeWebResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotSymbolRestClient, GetSymbolsRequest, SharedSpotSymbol[]>(
                this,
                client => client.GetSpotSymbolsOptions,
                request,
                async () =>
                {

            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedSpotSymbol[]>.Error(result);

            var resultData = result.Data.Select(s => new SharedSpotSymbol(s.Name.Split(new[] { '-' })[0], s.Name.Split(new[] { '-' })[1], s.Name, s.Status == SymbolStatus.Online)
            {
                MinTradeQuantity = s.MinOrderQuantity,
                MinNotionalValue = s.MinNotional,
                MaxTradeQuantity = s.MaxOrderQuantity,
                QuantityStep = s.StepSize,
                PriceStep = s.TickSize
            }).ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, resultData);
            return SharedExecutionResult<SharedSpotSymbol[]>.Ok(result, resultData);
        
                });
        }

        async Task<ExchangeResult<SharedSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<SharedSymbol[]>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<SharedSymbol[]>(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, baseAsset));
        }

        async Task<ExchangeResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbol));
        }

        async Task<ExchangeResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbolName));
        }
        #endregion

        #region Ticker client

        GetSpotTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName);
        Task<ExchangeWebResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotTickerRestClient, GetTickerRequest, SharedSpotTicker>(
                this,
                client => client.GetSpotTickerOptions,
                request,
                async () =>
                {

            var result = await ExchangeData.GetTickersAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedSpotTicker>.Error(result);

            var ticker = result.Data.Single();
            return SharedExecutionResult<SharedSpotTicker>.Ok(result, new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, ticker.Symbol), ticker.Symbol, ticker.LastPrice, ticker.HighPrice, ticker.LowPrice, ticker.Volume, decimal.Parse(ticker.PriceChangePercent.Substring(0, ticker.PriceChangePercent.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture))
            {
                QuoteVolume = ticker.QuoteVolume
            });
        
                });
        }

        GetSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } = new GetSpotTickersOptions(_exchangeName);
        Task<ExchangeWebResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotTickerRestClient, GetTickersRequest, SharedSpotTicker[]>(
                this,
                client => client.GetSpotTickersOptions,
                request,
                async () =>
                {

            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedSpotTicker[]>.Error(result);

            return SharedExecutionResult<SharedSpotTicker[]>.Ok(result, result.Data.Select(x => new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, decimal.Parse(x.PriceChangePercent.Substring(0, x.PriceChangePercent.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture))
            {
                QuoteVolume = x.QuoteVolume
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

            var resultTicker = await ExchangeData.GetBookPriceAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
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
            var result = await ExchangeData.GetTradeHistoryAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedTrade[]>.Error(result);

            return SharedExecutionResult<SharedTrade[]>.Ok(result, result.Data.Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.BuyerIsMaker ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray());
        
                });
        }

        #endregion

        #region Balance client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Spot, AccountTypeFilter.Funding);

        Task<ExchangeWebResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IBalanceRestClient, GetBalancesRequest, SharedBalance[]>(
                this,
                client => client.GetBalancesOptions,
                request,
                async () =>
                {

            if (request.AccountType == SharedAccountType.Funding)
            {
                var result = await Account.GetFundingBalancesAsync(ct: ct).ConfigureAwait(false);
                if (!result)
                    return SharedExecutionResult<SharedBalance[]>.Error(result);

                return SharedExecutionResult<SharedBalance[]>.Ok(result, result.Data.Select(x => new SharedBalance(x.Asset, x.Free, x.Total)).ToArray());                
            }
            else
            {
                var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
                if (!result)
                    return SharedExecutionResult<SharedBalance[]>.Error(result);

                return SharedExecutionResult<SharedBalance[]>.Ok(result, result.Data.Select(x => new SharedBalance(x.Asset, x.Free, x.Total)).ToArray());
            }
        
                });
        }

        #endregion

        #region Spot Order Client

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);

        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.OutputAsset;
        SharedOrderType[] ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        SharedTimeInForce[] ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.QuoteAsset,
                SharedQuantityType.BaseAsset);

        string ISpotOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(40);

        Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, PlaceSpotOrderRequest, SharedId>(
                this,
                client => client.PlaceSpotOrderOptions,
                request,
                async () =>
                {

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                (request.OrderType == SharedOrderType.Limit || request.OrderType == SharedOrderType.LimitMaker) ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: request.Quantity?.QuantityInBaseAsset ?? request.Quantity?.QuantityInContracts,
                quoteQuantity: request.Quantity?.QuantityInQuoteAsset,
                price: request.Price,
                timeInForce: GetTimeInForce(request.OrderType, request.TimeInForce),
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return SharedExecutionResult<SharedId>.Error(result);

            return SharedExecutionResult<SharedId>.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        
                });
        }

        EndpointOptions<GetOrderRequest, ISpotOrderRestClient> ISpotOrderRestClient.GetSpotOrderOptions { get; } = new EndpointOptions<GetOrderRequest, ISpotOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, GetOrderRequest, SharedSpotOrder>(
                this,
                client => client.GetSpotOrderOptions,
                request,
                async () =>
                {

            if (!long.TryParse(request.OrderId, out var orderId))
                return SharedExecutionResult<SharedSpotOrder>.Error(new ExchangeWebResult<SharedSpotOrder>(Exchange, ArgumentError.Invalid(nameof(GetOrderRequest.OrderId), "Invalid order id")));

            var order = await Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return SharedExecutionResult<SharedSpotOrder>.Error(order);

            return SharedExecutionResult<SharedSpotOrder>.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId.ToString(),
                ParseOrderType(order.Data.Type, null),
                order.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                OrderPrice = order.Data.Price,
                OrderQuantity = new SharedOrderQuantity(order.Data.Quantity, order.Data.QuoteQuantity, null),
                QuantityFilled = new SharedOrderQuantity(order.Data.QuantityFilled, order.Data.ValueFilled, null),
                Fee = Math.Abs(order.Data.Fee),
                FeeAsset = order.Data.FeeAsset,
                UpdateTime = order.Data.UpdateTime,
                AveragePrice = order.Data.AveragePrice == 0 ? null :order.Data.AveragePrice,
                TriggerPrice = order.Data.StopPrice,
                IsTriggerOrder = order.Data.StopPrice != null
            });
        
                });
        }

        EndpointOptions<GetOpenOrdersRequest, ISpotOrderRestClient> ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest, ISpotOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, GetOpenOrdersRequest, SharedSpotOrder[]>(
                this,
                client => client.GetOpenSpotOrdersOptions,
                request,
                async () =>
                {

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!orders)
                return SharedExecutionResult<SharedSpotOrder[]>.Error(orders);

            return SharedExecutionResult<SharedSpotOrder[]>.Ok(orders, orders.Data.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol),
                x.Symbol,
                x.OrderId.ToString(),
                ParseOrderType(x.Type, null),
                x.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                OrderPrice = x.Price,
                OrderQuantity = new SharedOrderQuantity(x.Quantity, x.QuoteQuantity, null),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.ValueFilled, null),
                Fee = Math.Abs(x.Fee),
                FeeAsset = x.FeeAsset,
                UpdateTime = x.UpdateTime,
                AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                TriggerPrice = x.StopPrice,
                IsTriggerOrder = x.StopPrice != null
            }).ToArray());
        
                });
        }

        GetSpotClosedOrdersOptions ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, false, true, true, 100);
        Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, GetClosedOrdersRequest, SharedSpotOrder[]>(
                this,
                client => client.GetClosedSpotOrdersOptions,
                request,
                async () =>
                {

            var direction = DataDirection.Descending;
            var limit = request.Limit ?? 100;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Pagination using orderId doesn't seem to work correctly
            var result = await Trading.GetOrdersAsync(
                symbol,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                page: pageParams.Page,
                pageSize: limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedSpotOrder[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                   () => Pagination.NextPageFromPage(pageParams),
                   result.Data.Length,
                   result.Data.Select(x => x.CreateTime),
                   request.StartTime,
                   request.EndTime ?? DateTime.UtcNow,
                   pageParams);

            return SharedExecutionResult<SharedSpotOrder[]>.Ok(result,
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedSpotOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol),
                            x.Symbol,
                            x.OrderId.ToString(),
                            ParseOrderType(x.Type, null),
                            x.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.CreateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price,
                            OrderQuantity = new SharedOrderQuantity(x.Quantity, x.QuoteQuantity, null),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.ValueFilled, null),
                            Fee = Math.Abs(x.Fee),
                            FeeAsset = x.FeeAsset,
                            UpdateTime = x.UpdateTime,
                            AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                            TriggerPrice = x.StopPrice,
                            IsTriggerOrder = x.StopPrice != null
                        })
                    .ToArray(), nextPageRequest);
        
                });
        }

        EndpointOptions<GetOrderTradesRequest, ISpotOrderRestClient> ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest, ISpotOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, GetOrderTradesRequest, SharedUserTrade[]>(
                this,
                client => client.GetSpotOrderTradesOptions,
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
                x.Id.ToString(),
                x.IsBuyer ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Role = x.IsMaker ? SharedRole.Maker : SharedRole.Taker,
                Fee = Math.Abs(x.Fee),
                FeeAsset = x.FeeAsset,
            }).ToArray());
        
                });
        }

        GetSpotUserTradesOptions ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new GetSpotUserTradesOptions(_exchangeName, true, false, true, 1000);
        Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, GetUserTradesRequest, SharedUserTrade[]>(
                this,
                client => client.GetSpotUserTradesOptions,
                request,
                async () =>
                {

            var direction = DataDirection.Ascending;
            var limit = request.Limit ?? 1000;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(
                direction, limit, request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageRequest);

            // Get data
            var result = await Trading.GetUserTradesAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.FromId != null ? null : pageParams.StartTime,
                endTime: pageParams.FromId != null ? null : pageParams.EndTime,
                limit: limit,
                fromId: pageParams.FromId == null ? null : long.Parse(pageParams.FromId),
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedUserTrade[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromId(result.Data.Max(x => x.Id)),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return SharedExecutionResult<SharedUserTrade[]>.Ok(result,
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol),
                            x.Symbol,
                            x.OrderId.ToString(),
                            x.Id.ToString(),
                            x.IsBuyer ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.Quantity,
                            x.Price,
                            x.Timestamp)
                        {
                            Role = x.IsMaker ? SharedRole.Maker : SharedRole.Taker,
                            Fee = Math.Abs(x.Fee),
                            FeeAsset = x.FeeAsset,
                        })
                    .ToArray(), nextPageRequest);
        
                });
        }

        EndpointOptions<CancelOrderRequest, ISpotOrderRestClient> ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new EndpointOptions<CancelOrderRequest, ISpotOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, CancelOrderRequest, SharedId>(
                this,
                client => client.CancelSpotOrderOptions,
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

        private SharedOrderType ParseOrderType(OrderType type, TimeInForce? tif)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.Limit && tif == TimeInForce.PostOnly) return SharedOrderType.LimitMaker;
            if (type == OrderType.Limit) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private Enums.TimeInForce? GetTimeInForce(SharedOrderType type, SharedTimeInForce? tif)
        {
            if (type == SharedOrderType.LimitMaker) return Enums.TimeInForce.PostOnly;
            if (tif == SharedTimeInForce.ImmediateOrCancel) return Enums.TimeInForce.ImmediateOrCancel;
            if (tif == SharedTimeInForce.GoodTillCanceled) return Enums.TimeInForce.GoodTillCanceled;

            return null;
        }

        #endregion

        #region Spot Client Id Order Client

        EndpointOptions<GetOrderRequest, ISpotOrderClientIdRestClient> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdOptions { get; } = new EndpointOptions<GetOrderRequest, ISpotOrderClientIdRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderClientIdRestClient, GetOrderRequest, SharedSpotOrder>(
                this,
                client => client.GetSpotOrderByClientOrderIdOptions,
                request,
                async () =>
                {

            var order = await Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return SharedExecutionResult<SharedSpotOrder>.Error(order);

            return SharedExecutionResult<SharedSpotOrder>.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId.ToString(),
                ParseOrderType(order.Data.Type, null),
                order.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                OrderPrice = order.Data.Price,
                OrderQuantity = new SharedOrderQuantity(order.Data.Quantity, order.Data.QuoteQuantity, null),
                QuantityFilled = new SharedOrderQuantity(order.Data.QuantityFilled, order.Data.ValueFilled, null),
                Fee = Math.Abs(order.Data.Fee),
                FeeAsset = order.Data.FeeAsset,
                UpdateTime = order.Data.UpdateTime,
                AveragePrice = order.Data.AveragePrice == 0 ? null : order.Data.AveragePrice,
                TriggerPrice = order.Data.StopPrice,
                IsTriggerOrder = order.Data.StopPrice != null
            });
        
                });
        }

        EndpointOptions<CancelOrderRequest, ISpotOrderClientIdRestClient> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdOptions { get; } = new EndpointOptions<CancelOrderRequest, ISpotOrderClientIdRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderClientIdRestClient, CancelOrderRequest, SharedId>(
                this,
                client => client.CancelSpotOrderByClientOrderIdOptions,
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

        #region Asset client

        EndpointOptions<GetAssetRequest, IAssetsRestClient> IAssetsRestClient.GetAssetOptions { get; } = new EndpointOptions<GetAssetRequest, IAssetsRestClient>(_exchangeName, false);
        Task<ExchangeWebResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IAssetsRestClient, GetAssetRequest, SharedAsset>(
                this,
                client => client.GetAssetOptions,
                request,
                async () =>
                {

            var result = await Account.GetAssetsAsync(request.Asset, ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedAsset>.Ok(result, default);

            var asset = result.Data.SingleOrDefault();
            if (asset == null)
                return SharedExecutionResult<SharedAsset>.Error(new ServerError(new ErrorInfo(ErrorType.UnknownAsset, "Asset not found")));

            return SharedExecutionResult<SharedAsset>.Ok(result, new SharedAsset(asset.Asset)
            {
                FullName = asset.Name,
                Networks = asset.Networks.Select(x => new SharedAssetNetwork(x.Network)
                {
                    DepositEnabled = x.DepositEnabled,
                    MaxWithdrawQuantity = x.MaxWithdraw,
                    MinConfirmations = x.MinConfirmations,
                    MinWithdrawQuantity = x.MinWithdraw,
                    WithdrawEnabled = x.WithdrawEnabled,
                    WithdrawFee = x.WithdrawFee,
                    ContractAddress = x.ContractAddress
                }).ToArray()
            });
        
                });
        }

        EndpointOptions<GetAssetsRequest, IAssetsRestClient> IAssetsRestClient.GetAssetsOptions { get; } = new EndpointOptions<GetAssetsRequest, IAssetsRestClient>(_exchangeName, true);

        Task<ExchangeWebResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IAssetsRestClient, GetAssetsRequest, SharedAsset[]>(
                this,
                client => client.GetAssetsOptions,
                request,
                async () =>
                {

            var result = await Account.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedAsset[]>.Error(result);

            return SharedExecutionResult<SharedAsset[]>.Ok(result, result.Data.Select(x => new SharedAsset(x.Asset)
            {
                FullName = x.Name,
                Networks = x.Networks.Select(x => new SharedAssetNetwork(x.Network)
                {
                    DepositEnabled = x.DepositEnabled,
                    MaxWithdrawQuantity = x.MaxWithdraw,
                    MinConfirmations = x.MinConfirmations,
                    MinWithdrawQuantity = x.MinWithdraw,
                    WithdrawEnabled = x.WithdrawEnabled,
                    WithdrawFee = x.WithdrawFee,
                    ContractAddress = x.ContractAddress
                }).ToArray()
            }).ToArray());
        
                });
        }

        #endregion

        #region Deposit client
        EndpointOptions<GetDepositAddressesRequest, IDepositRestClient> IDepositRestClient.GetDepositAddressesOptions { get; } = new EndpointOptions<GetDepositAddressesRequest, IDepositRestClient>(_exchangeName, true);

        Task<ExchangeWebResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IDepositRestClient, GetDepositAddressesRequest, SharedDepositAddress[]>(
                this,
                client => client.GetDepositAddressesOptions,
                request,
                async () =>
                {

            var depositAddresses = await Account.GetDepositAddressAsync(request.Asset, ct: ct).ConfigureAwait(false);
            if (!depositAddresses)
                return SharedExecutionResult<SharedDepositAddress[]>.Error(depositAddresses);

            var result = depositAddresses.Data.Data;
            if (request.Network != null)
                result = result.Where(r => r.Network == request.Network).ToArray();

            return SharedExecutionResult<SharedDepositAddress[]>.Ok(depositAddresses, result.Select(x => new SharedDepositAddress(x.Asset, x.Address)
            {
                Network = x.Network
            }
            ).ToArray());
        
                });
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(_exchangeName, false, true, true, 1000);
        Task<ExchangeWebResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IDepositRestClient, GetDepositsRequest, SharedDeposit[]>(
                this,
                client => client.GetDepositsOptions,
                request,
                async () =>
                {

            var limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Account.GetDepositHistoryAsync(
                request.Asset,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: limit,
                offset: pageParams.Offset,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedDeposit[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromOffset(pageParams, limit),
                    result.Data.Length,
                    result.Data.Select(x => x.InsertTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            return SharedExecutionResult<SharedDeposit[]>.Ok(result,
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.InsertTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedDeposit(
                            x.Asset,
                            x.Quantity,
                            x.Status == DepositStatus.Completed,
                            x.InsertTime,
                            ParseTransferStatus(x.Status)
                            )
                        {
                            Confirmations = x.ConfirmedTimes.Contains("/") ? int.Parse(x.ConfirmedTimes.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) : null,
                            Tag = x.AddressTag,
                            TransactionId = x.TransactionId,
                            Network = x.Network,
                        })
                    .ToArray(), nextPageRequest);
        
                });
        }

        private SharedTransferStatus ParseTransferStatus(DepositStatus status)
        {
            if (status == DepositStatus.Completed)
                return SharedTransferStatus.Completed;
            if (status == DepositStatus.InProgress || status == DepositStatus.ChainUploaded)
                return SharedTransferStatus.InProgress;
            
            return SharedTransferStatus.Unknown;
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 1, 2000, false);
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

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(_exchangeName, false, true, true, 1000);
        Task<ExchangeWebResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IWithdrawalRestClient, GetWithdrawalsRequest, SharedWithdrawal[]>(
                this,
                client => client.GetWithdrawalsOptions,
                request,
                async () =>
                {

            var limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Account.GetWithdrawalHistoryAsync(
                request.Asset,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: limit,
                offset: pageParams.Offset,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedWithdrawal[]>.Error(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromOffset(pageParams, limit),
                    result.Data.Length,
                    result.Data.Select(x => x.ApplyTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            return SharedExecutionResult<SharedWithdrawal[]>.Ok(result,
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.ApplyTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedWithdrawal(x.Asset, x.Address, x.Quantity, x.Status == WithdrawalStatus.Completed, x.ApplyTime)
                        {
                            Id = x.Id,
                            Confirmations = x.Confirmations,
                            Network = x.Network,
                            Tag = x.AddressTag,
                            TransactionId = x.TransactionId,
                            Fee = x.Fee
                        })
                    .ToArray(), nextPageRequest);
        
                });
        }

        #endregion

        #region Withdraw client

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions(_exchangeName);

        Task<ExchangeWebResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<IWithdrawRestClient, WithdrawRequest, SharedId>(
                this,
                client => client.WithdrawOptions,
                request,
                async () =>
                {

            // Get data
            var withdrawal = await Account.WithdrawAsync(
                request.Asset,
                request.Address,
                request.Quantity,
                AccountType.Funding,
                network: request.Network,
                addressTag: request.AddressTag,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal)
                return SharedExecutionResult<SharedId>.Error(withdrawal);

            return SharedExecutionResult<SharedId>.Ok(withdrawal, new SharedId(withdrawal.Data.Id));
        
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
            var result = await Account.GetTradingFeesAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedFee>.Error(result);

            // Return
            return SharedExecutionResult<SharedFee>.Ok(result, new SharedFee(result.Data.MakerFeeRate * 100, result.Data.TakerFeeRate * 100));
        
                });
        }
        #endregion

        #region Trigger Order Client
        PlaceSpotTriggerOrderOptions ISpotTriggerOrderRestClient.PlaceSpotTriggerOrderOptions { get; } = new PlaceSpotTriggerOrderOptions(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> ISpotTriggerOrderRestClient.PlaceSpotTriggerOrderAsync(PlaceSpotTriggerOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotOrderRestClient, PlaceSpotTriggerOrderRequest, SharedId>(
                this,
                client => ((ISpotTriggerOrderRestClient)this).PlaceSpotTriggerOrderOptions,
                request,
                async () =>
                {

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.OrderSide == SharedOrderSide.Buy ? OrderSide.Buy: OrderSide.Sell,
                request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit,
                quantity: request.Quantity.QuantityInBaseAsset,
                quoteQuantity: request.Quantity.QuantityInQuoteAsset,
                timeInForce: request.OrderPrice == null ? TimeInForce.ImmediateOrCancel : TimeInForce.GoodTillCanceled,
                price: request.OrderPrice,
                stopPrice: request.TriggerPrice,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedId>.Error(result);

            // Return
            return SharedExecutionResult<SharedId>.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        
                });
        }

        EndpointOptions<GetOrderRequest, ISpotTriggerOrderRestClient> ISpotTriggerOrderRestClient.GetSpotTriggerOrderOptions { get; } = new EndpointOptions<GetOrderRequest, ISpotTriggerOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedSpotTriggerOrder>> ISpotTriggerOrderRestClient.GetSpotTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotTriggerOrderRestClient, GetOrderRequest, SharedSpotTriggerOrder>(
                this,
                client => client.GetSpotTriggerOrderOptions,
                request,
                async () =>
                {

            if (!long.TryParse(request.OrderId, out var id))
                throw new ArgumentException($"Invalid order id");

            var result = await Trading.GetOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                id,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return SharedExecutionResult<SharedSpotTriggerOrder>.Error(result);

            var (orderType, orderDirection) = ParseTriggerDirections(result.Data.Type, result.Data.Side);
            // Return
            return SharedExecutionResult<SharedSpotTriggerOrder>.Ok(result, new SharedSpotTriggerOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, result.Data.Symbol),
                result.Data.Symbol,
                result.Data.OrderId.ToString(),
                orderType,
                orderDirection,
                ParseTriggerStatus(result.Data),
                result.Data.StopPrice ?? 0,
                result.Data.CreateTime
                )
            {
                PlacedOrderId = result.Data.OrderId.ToString(),
                OrderPrice = result.Data.Price,
                OrderQuantity = new SharedOrderQuantity(result.Data.Quantity, result.Data.QuoteQuantity, null),
                QuantityFilled = new SharedOrderQuantity(result.Data.QuantityFilled, result.Data.ValueFilled, null),
                Fee = Math.Abs(result.Data.Fee),
                FeeAsset = result.Data.FeeAsset,
                UpdateTime = result.Data.UpdateTime,
                AveragePrice = result.Data.AveragePrice == 0 ? null : result.Data.AveragePrice,
                ClientOrderId = result.Data.ClientOrderId
            });
        
                });
        }

        private SharedTriggerOrderStatus ParseTriggerStatus(BingXOrderDetails data)
        {
            if (data.Status == OrderStatus.Filled)
                return SharedTriggerOrderStatus.Filled;

            if (data.Status == OrderStatus.Canceled || data.Status == OrderStatus.Failed)
                return SharedTriggerOrderStatus.CanceledOrRejected;

            if (data.Status == OrderStatus.New || data.Status == OrderStatus.PartiallyFilled || data.Status == OrderStatus.Working)
                return SharedTriggerOrderStatus.Active;

            return SharedTriggerOrderStatus.Unknown;
        }

        EndpointOptions<CancelOrderRequest, ISpotTriggerOrderRestClient> ISpotTriggerOrderRestClient.CancelSpotTriggerOrderOptions { get; } = new EndpointOptions<CancelOrderRequest, ISpotTriggerOrderRestClient>(_exchangeName, true);
        Task<ExchangeWebResult<SharedId>> ISpotTriggerOrderRestClient.CancelSpotTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ISpotTriggerOrderRestClient, CancelOrderRequest, SharedId>(
                this,
                client => client.CancelSpotTriggerOrderOptions,
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

        private (SharedOrderType, SharedTriggerOrderDirection) ParseTriggerDirections(OrderType orderType, OrderSide side)
        {
            if (side == OrderSide.Buy)
            {
                return (
                    orderType == OrderType.StopMarket ? SharedOrderType.Market : SharedOrderType.Limit,
                    SharedTriggerOrderDirection.Enter);
            }
            else
            {
                return (
                    orderType == OrderType.StopMarket ? SharedOrderType.Market : SharedOrderType.Limit,
                    SharedTriggerOrderDirection.Exit);
            }            
        }
        #endregion

        #region Transfer client

        TransferOptions ITransferRestClient.TransferOptions { get; } = new TransferOptions(_exchangeName, [
            SharedAccountType.Funding,
            SharedAccountType.Spot,
            SharedAccountType.PerpetualLinearFutures,
            SharedAccountType.PerpetualInverseFutures
            ]);
        Task<ExchangeWebResult<SharedId>> ITransferRestClient.TransferAsync(TransferRequest request, CancellationToken ct)
        {
            return SharedUtils.ExecuteSharedAsync<ITransferRestClient, TransferRequest, SharedId>(
                this,
                client => client.TransferOptions,
                request,
                async () =>
                {

            var fromAccount = GetTransferType(request.FromAccountType);
            var toAccount = GetTransferType(request.ToAccountType);
            if (fromAccount == null || toAccount == null)
                return SharedExecutionResult<SharedId>.Error(new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Invalid("To/From AccountType", "invalid to/from account combination")));

            // Get data
            var transfer = await Account.TransferAsync(
                request.Asset,
                request.Quantity,
                fromAccount.Value,
                toAccount.Value,
                ct: ct).ConfigureAwait(false);
            if (!transfer)
                return SharedExecutionResult<SharedId>.Error(transfer);

            return SharedExecutionResult<SharedId>.Ok(transfer, new SharedId(transfer.Data.TransactionId.ToString()));
        
                });
        }

        private TransferAccountType? GetTransferType(SharedAccountType type)
        {
            if (type == SharedAccountType.Funding) return TransferAccountType.Funding;
            if (type == SharedAccountType.Spot) return TransferAccountType.Spot;
            if (type == SharedAccountType.PerpetualLinearFutures) return TransferAccountType.UsdtPerpetualFutures;
            if (type == SharedAccountType.PerpetualInverseFutures) return TransferAccountType.CoinPerpetualFutures;
            return null;
        }

        #endregion
    }
}
