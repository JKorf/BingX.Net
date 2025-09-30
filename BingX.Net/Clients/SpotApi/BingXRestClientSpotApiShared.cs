using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXRestClientSpotApi : IBingXRestClientSpotApiShared
    {
        private const string _topicId = "BingXSpot";

        public string Exchange => BingXExchange.ExchangeName;
        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };
        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Kline client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 1000, false);

        async Task<ExchangeWebResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedKline[]>(Exchange, validationError);

            // Determine pagination
            // Data is normally returned oldest first, so to do newest first pagination we have to do some calc
            DateTime endTime = request.EndTime ?? DateTime.UtcNow;
            DateTime? startTime = request.StartTime;
            if (pageToken is DateTimeToken dateTimeToken)
                endTime = dateTimeToken.LastTime;

            var limit = request.Limit ?? 1000;
            if (startTime == null || startTime < endTime)
            {
                var offset = (int)interval * limit;
                startTime = endTime.AddSeconds(-offset);
            }

            if (startTime < request.StartTime)
                startTime = request.StartTime;

            // Get data
            var result = await ExchangeData.GetKlinesAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                interval,
                startTime,
                endTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedKline[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (result.Data.Count() == limit)
            {
                var minOpenTime = result.Data.Min(x => x.OpenTime);
                if (request.StartTime == null || minOpenTime > request.StartTime.Value)
                    nextToken = new DateTimeToken(minOpenTime.AddSeconds(-(int)interval));
            }

            return result.AsExchangeResult<SharedKline[]>(Exchange, request.Symbol.TradingMode, result.Data.Select(x => new SharedKline(x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)).ToArray(), nextToken);
        }

        #endregion

        #region Spot Symbol client

        EndpointOptions<GetSymbolsRequest> ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);
        async Task<ExchangeWebResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotSymbolRestClient)this).GetSpotSymbolsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, null, default);

            var response = result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, TradingMode.Spot, result.Data.Select(s => new SharedSpotSymbol(s.Name.Split(new[] { '-' })[0], s.Name.Split(new[] { '-' })[1], s.Name, s.Status == SymbolStatus.Online)
            {
                MinTradeQuantity = s.MinOrderQuantity,
                MinNotionalValue = s.MinNotional,
                MaxTradeQuantity = s.MaxOrderQuantity,
                QuantityStep = s.StepSize,
                PriceStep = s.TickSize
            }).ToArray());

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, response.Data);
            return response;
        }

        #endregion

        #region Ticker client

        EndpointOptions<GetTickerRequest> ISpotTickerRestClient.GetSpotTickerOptions { get; } = new EndpointOptions<GetTickerRequest>(false);
        async Task<ExchangeWebResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickerOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker>(Exchange, null, default);

            var ticker = result.Data.Single();
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, ticker.Symbol), ticker.Symbol, ticker.LastPrice, ticker.HighPrice, ticker.LowPrice, ticker.Volume, decimal.Parse(ticker.PriceChangePercent.Substring(0, ticker.PriceChangePercent.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture))
            {
                QuoteVolume = ticker.QuoteVolume
            });
        }

        EndpointOptions<GetTickersRequest> ISpotTickerRestClient.GetSpotTickersOptions { get; } = new EndpointOptions<GetTickersRequest>(false);
        async Task<ExchangeWebResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickersOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker[]>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedSpotTicker[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, decimal.Parse(x.PriceChangePercent.Substring(0, x.PriceChangePercent.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture))
            {
                QuoteVolume = x.QuoteVolume
            }).ToArray());
        }

        #endregion

        #region Book Ticker client

        EndpointOptions<GetBookTickerRequest> IBookTickerRestClient.GetBookTickerOptions { get; } = new EndpointOptions<GetBookTickerRequest>(false);
        async Task<ExchangeWebResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IBookTickerRestClient)this).GetBookTickerOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBookTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetBookPriceAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedBookTicker>(Exchange, null, default);

            return resultTicker.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, resultTicker.Data.Symbol),
                resultTicker.Data.Symbol,
                resultTicker.Data.BestAskPrice,
                resultTicker.Data.BestAskQuantity,
                resultTicker.Data.BestBidPrice,
                resultTicker.Data.BestBidQuantity));
        }

        #endregion

        #region Recent Trade client
        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(1000, false);

        async Task<ExchangeWebResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedTrade[]>(Exchange, validationError);

            var result = await ExchangeData.GetTradeHistoryAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTrade[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedTrade[]>(Exchange, request.Symbol.TradingMode, result.Data.Select(x => new SharedTrade(x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.BuyerIsMaker ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray());
        }

        #endregion

        #region Balance client
        EndpointOptions<GetBalancesRequest> IBalanceRestClient.GetBalancesOptions { get; } = new EndpointOptions<GetBalancesRequest>(true);

        async Task<ExchangeWebResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedBalance[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedBalance[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedBalance(x.Asset, x.Free, x.Total)).ToArray());
        }

        #endregion

        #region Spot Order Client

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions();

        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.OutputAsset;
        SharedOrderType[] ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        SharedTimeInForce[] ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.QuoteAsset,
                SharedQuantityType.BaseAsset);

        string ISpotOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(40);

        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).PlaceSpotOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.TradingMode,
                SupportedTradingModes,
                ((ISpotOrderRestClient)this).SpotSupportedOrderTypes,
                ((ISpotOrderRestClient)this).SpotSupportedTimeInForce,
                ((ISpotOrderRestClient)this).SpotSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

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
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(result.Data.OrderId.ToString()));
        }

        EndpointOptions<GetOrderRequest> ISpotOrderRestClient.GetSpotOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, ArgumentError.Invalid(nameof(GetOrderRequest.OrderId), "Invalid order id"));

            var order = await Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotOrder(
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
        }

        EndpointOptions<GetOpenOrdersRequest> ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetOpenSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, null, default);

            return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedSpotOrder(
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
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, true, 100, true);
        async Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetClosedSpotOrdersOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder[]>(Exchange, validationError);

            // Determine page token
            int page = 1;
            int pageSize = request.Limit ?? 100;
            if (pageToken is PageToken token)
            {
                page = token.Page;
                pageSize = token.PageSize;
            }

            // Get data
            var orders = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: request.EndTime,
                page: page,
                pageSize: pageSize,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, null, default);

            // Get next token
            PageToken? nextToken = null;
            if (orders.Data.Count() == pageSize)
                nextToken = new PageToken(page + 1, pageSize);

            return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedSpotOrder(
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
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, ArgumentError.Invalid(nameof(GetOrderTradesRequest.OrderId), "Invalid order id"));

            var orders = await Trading.GetUserTradesAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId: orderId, ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, request.Symbol.TradingMode,orders.Data.Select(x => new SharedUserTrade(
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
        }

        PaginatedEndpointOptions<GetUserTradesRequest> ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Ascending, true, 1000, true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotUserTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            long? fromId = null;
            if (pageToken is FromIdToken fromIdToken)
                fromId = long.Parse(fromIdToken.FromToken);

            // Get data
            var orders = await Trading.GetUserTradesAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                startTime: fromId != null ? null: request.StartTime ?? DateTime.UtcNow.AddDays(-7),
                endTime: fromId != null ? null : request.EndTime ?? DateTime.UtcNow,
                limit: request.Limit ?? 500,
                fromId: fromId,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            // Get next token
            FromIdToken? nextToken = null;
            if (orders.Data.Count() == (request.Limit ?? 500))
                nextToken = new FromIdToken(orders.Data.Max(o => o.Id + 1).ToString());

            return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, request.Symbol.TradingMode,orders.Data.Select(x => new SharedUserTrade(
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
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id"));

            var order = await Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(order.Data.OrderId.ToString()));
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Pending || status == OrderStatus.PartiallyFilled || status == OrderStatus.New) return SharedOrderStatus.Open;
            if (status == OrderStatus.Canceled || status == OrderStatus.Failed) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
        }

        private SharedOrderType ParseOrderType(OrderType type, TimeInForce? tif)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.Limit && tif == TimeInForce.PostOnly) return SharedOrderType.LimitMaker;
            if (type == OrderType.Limit) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private SharedTimeInForce? ParseTimeInForce(TimeInForce? tif)
        {
            if (tif == TimeInForce.GoodTillCanceled) return SharedTimeInForce.GoodTillCanceled;
            if (tif == TimeInForce.ImmediateOrCancel) return SharedTimeInForce.ImmediateOrCancel;
            if (tif == TimeInForce.FillOrKill) return SharedTimeInForce.FillOrKill;

            return null;
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

        EndpointOptions<GetOrderRequest> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotOrder(
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
        }

        EndpointOptions<CancelOrderRequest> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(order.Data.OrderId.ToString()));
        }
        #endregion

        #region Asset client

        EndpointOptions<GetAssetRequest> IAssetsRestClient.GetAssetOptions { get; } = new EndpointOptions<GetAssetRequest>(false);
        async Task<ExchangeWebResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedAsset>(Exchange, validationError);

            var result = await Account.GetAssetsAsync(request.Asset, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedAsset>(Exchange, TradingMode.Spot, default);

            var asset = result.Data.SingleOrDefault();
            if (asset == null)
                return result.AsExchangeError<SharedAsset>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownAsset, "Asset not found")));

            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedAsset(asset.Asset)
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
        }

        EndpointOptions<GetAssetsRequest> IAssetsRestClient.GetAssetsOptions { get; } = new EndpointOptions<GetAssetsRequest>(true);

        async Task<ExchangeWebResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedAsset[]>(Exchange, validationError);

            var result = await Account.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedAsset[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedAsset[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedAsset(x.Asset)
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
        }

        #endregion

        #region Deposit client
        EndpointOptions<GetDepositAddressesRequest> IDepositRestClient.GetDepositAddressesOptions { get; } = new EndpointOptions<GetDepositAddressesRequest>(true);

        async Task<ExchangeWebResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositAddressesOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await Account.GetDepositAddressAsync(request.Asset, ct: ct).ConfigureAwait(false);
            if (!depositAddresses)
                return depositAddresses.AsExchangeResult<SharedDepositAddress[]>(Exchange, null, default);

            var result = depositAddresses.Data.Data;
            if (request.Network != null)
                result = result.Where(r => r.Network == request.Network).ToArray();

            return depositAddresses.AsExchangeResult<SharedDepositAddress[]>(Exchange, TradingMode.Spot, result.Select(x => new SharedDepositAddress(x.Asset, x.Address)
            {
                Network = x.Network
            }
            ).ToArray());
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(SharedPaginationSupport.Descending, true, 1000);
        async Task<ExchangeWebResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedDeposit[]>(Exchange, validationError);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            // Get data
            var result = await Account.GetDepositHistoryAsync(request.Asset,
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit ?? 100,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedDeposit[]>(Exchange, null, default);

            // Determine next token
            OffsetToken? nextToken = null;
            if (result.Data.Count() == (request.Limit ?? 100))
                nextToken = new OffsetToken((offset ?? 0) + result.Data.Count());

            return result.AsExchangeResult<SharedDeposit[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedDeposit(x.Asset, x.Quantity, x.Status == DepositStatus.Completed, x.InsertTime)
            {
                Confirmations = x.ConfirmedTimes.Contains("/") ? int.Parse(x.ConfirmedTimes.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) : null,
                Tag = x.AddressTag,
                TransactionId = x.TransactionId,
                Network = x.Network,
            }).ToArray(), nextToken);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 2000, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }
        #endregion

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(SharedPaginationSupport.Descending, true, 1000);
        async Task<ExchangeWebResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IWithdrawalRestClient)this).GetWithdrawalsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedWithdrawal[]>(Exchange, validationError);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            // Get data
            var withdrawals = await Account.GetWithdrawalHistoryAsync(
                request.Asset,
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit ?? 100,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!withdrawals)
                return withdrawals.AsExchangeResult<SharedWithdrawal[]>(Exchange, null, default);

            // Determine next token
            OffsetToken? nextToken = null;
            if (withdrawals.Data.Count() == (request.Limit ?? 100))
                nextToken = new OffsetToken((offset ?? 0) + withdrawals.Data.Count());

            return withdrawals.AsExchangeResult<SharedWithdrawal[]>(Exchange, TradingMode.Spot, withdrawals.Data.Select(x => new SharedWithdrawal(x.Asset, x.Address, x.Quantity, x.Status == WithdrawalStatus.Completed, x.ApplyTime)
            {
                Id = x.Id,
                Confirmations = x.Confirmations,
                Network = x.Network,
                Tag = x.AddressTag,
                TransactionId = x.TransactionId,
                Fee = x.Fee
            }).ToArray(), nextToken);
        }

        #endregion

        #region Withdraw client

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions();

        async Task<ExchangeWebResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = ((IWithdrawRestClient)this).WithdrawOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

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
                return withdrawal.AsExchangeResult<SharedId>(Exchange, null, default);

            return withdrawal.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(withdrawal.Data.Id));
        }

        #endregion

        #region Listen Key client

        EndpointOptions<StartListenKeyRequest> IListenKeyRestClient.StartOptions { get; } = new EndpointOptions<StartListenKeyRequest>(true);
        async Task<ExchangeWebResult<string>> IListenKeyRestClient.StartListenKeyAsync(StartListenKeyRequest request, CancellationToken ct)
        {
            var validationError = ((IListenKeyRestClient)this).StartOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<string>(Exchange, validationError);

            // Get data
            var result = await Account.StartUserStreamAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<string>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, result.Data);
        }
        EndpointOptions<KeepAliveListenKeyRequest> IListenKeyRestClient.KeepAliveOptions { get; } = new EndpointOptions<KeepAliveListenKeyRequest>(true);
        async Task<ExchangeWebResult<string>> IListenKeyRestClient.KeepAliveListenKeyAsync(KeepAliveListenKeyRequest request, CancellationToken ct)
        {
            var validationError = ((IListenKeyRestClient)this).KeepAliveOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<string>(Exchange, validationError);

            // Get data
            var result = await Account.KeepAliveUserStreamAsync(request.ListenKey, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<string>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, request.ListenKey);
        }

        EndpointOptions<StopListenKeyRequest> IListenKeyRestClient.StopOptions { get; } = new EndpointOptions<StopListenKeyRequest>(true);
        async Task<ExchangeWebResult<string>> IListenKeyRestClient.StopListenKeyAsync(StopListenKeyRequest request, CancellationToken ct)
        {
            var validationError = ((IListenKeyRestClient)this).StopOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<string>(Exchange, validationError);

            // Get data
            var result = await Account.StopUserStreamAsync(request.ListenKey, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<string>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, request.ListenKey);
        }
        #endregion

        #region Fee Client
        EndpointOptions<GetFeeRequest> IFeeRestClient.GetFeeOptions { get; } = new EndpointOptions<GetFeeRequest>(true);

        async Task<ExchangeWebResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = ((IFeeRestClient)this).GetFeeOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFee>(Exchange, validationError);

            // Get data
            var result = await Account.GetTradingFeesAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFee>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedFee(result.Data.MakerFeeRate * 100, result.Data.TakerFeeRate * 100));
        }
        #endregion

        #region Trigger Order Client
        PlaceSpotTriggerOrderOptions ISpotTriggerOrderRestClient.PlaceSpotTriggerOrderOptions { get; } = new PlaceSpotTriggerOrderOptions(true);
        async Task<ExchangeWebResult<SharedId>> ISpotTriggerOrderRestClient.PlaceSpotTriggerOrderAsync(PlaceSpotTriggerOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTriggerOrderRestClient)this).PlaceSpotTriggerOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

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
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(result.Data.OrderId.ToString()));
        }

        EndpointOptions<GetOrderRequest> ISpotTriggerOrderRestClient.GetSpotTriggerOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotTriggerOrder>> ISpotTriggerOrderRestClient.GetSpotTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTriggerOrderRestClient)this).GetSpotTriggerOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTriggerOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var id))
                throw new ArgumentException($"Invalid order id");

            var result = await Trading.GetOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                id,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTriggerOrder>(Exchange, null, default);

            var (orderType, orderDirection) = ParseTriggerDirections(result.Data.Type, result.Data.Side);
            // Return
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotTriggerOrder(
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
        }

        private SharedTriggerOrderStatus ParseTriggerStatus(BingXOrderDetails data)
        {
            if (data.Status == OrderStatus.Filled)
                return SharedTriggerOrderStatus.Filled;

            if (data.Status == OrderStatus.Canceled || data.Status == OrderStatus.Failed)
                return SharedTriggerOrderStatus.CanceledOrRejected;

            return SharedTriggerOrderStatus.Active;
        }

        EndpointOptions<CancelOrderRequest> ISpotTriggerOrderRestClient.CancelSpotTriggerOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotTriggerOrderRestClient.CancelSpotTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTriggerOrderRestClient)this).CancelSpotTriggerOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id"));

            var order = await Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(order.Data.OrderId.ToString()));
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

    }
}
