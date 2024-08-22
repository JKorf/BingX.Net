using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis.Enums;
using CryptoExchange.Net.SharedApis.Interfaces;
using CryptoExchange.Net.SharedApis.Models;
using CryptoExchange.Net.SharedApis.Models.FilterOptions;
using CryptoExchange.Net.SharedApis.Models.Rest;
using CryptoExchange.Net.SharedApis.RequestModels;
using CryptoExchange.Net.SharedApis.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXRestClientSpotApi : IBingXRestClientSpotApiShared
    {
        public string Exchange => BingXExchange.ExchangeName;

        #region Kline client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(true, false)
        {
            MaxRequestDataPoints = 1000
        };

        async Task<ExchangeWebResult<IEnumerable<SharedKline>>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, new ArgumentError("Interval not supported"));

            // Determine page token
            DateTime? fromTimestamp = null;
            if (pageToken is DateTimeToken dateTimeToken)
                fromTimestamp = dateTimeToken.LastTime;

            var startTime = request.Filter?.StartTime;
            var endTime = request.Filter?.EndTime?.AddSeconds(-1);
            var apiLimit = 1000;

            // API returns the newest data first if the timespan is bigger than the api limit of 1000 results
            // So we need to request the first 1000 from the start time, then the 1000 after that etc
            if (request.Filter?.StartTime != null)
            {
                // Not paginated, check if the data will fit
                var seconds = apiLimit * (int)request.Interval;
                var maxEndTime = (fromTimestamp ?? request.Filter.StartTime).Value.AddSeconds(seconds - 1);
                if (maxEndTime < endTime)
                    endTime = maxEndTime;
            }

            // Get data
            var result = await ExchangeData.GetKlinesAsync(
                request.GetSymbol(FormatSymbol),
                interval,
                fromTimestamp ?? request.Filter?.StartTime,
                endTime,
                limit: request.Filter?.Limit ?? apiLimit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedKline>>(Exchange, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (request.Filter?.StartTime != null && result.Data.Any())
            {
                var maxOpenTime = result.Data.Max(x => x.OpenTime);
                if (maxOpenTime < request.Filter.EndTime!.Value.AddSeconds(-(int)request.Interval))
                    nextToken = new DateTimeToken(maxOpenTime.AddSeconds((int)interval));
            }

            // Reverse as data is returned in desc order instead of standard asc
            return result.AsExchangeResult(Exchange, result.Data.Reverse().Select(x => new SharedKline(x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)), nextToken);
        }

        #endregion

        #region Spot Symbol client

        async Task<ExchangeWebResult<IEnumerable<SharedSpotSymbol>>> ISpotSymbolRestClient.GetSpotSymbolsAsync(ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedSpotSymbol>>(Exchange, default);

            return result.AsExchangeResult(Exchange, result.Data.Select(s => new SharedSpotSymbol(s.Name.Split(new[] { '-' })[0], s.Name.Split(new[] { '-' })[1], s.Name)
            {
                MinTradeQuantity = s.MinOrderQuantity,
                MaxTradeQuantity = s.MaxOrderQuantity,
                QuantityStep = s.StepSize,
                PriceStep = s.TickSize
            }));
        }

        #endregion

        #region Ticker client

        async Task<ExchangeWebResult<SharedTicker>> ITickerRestClient.GetTickerAsync(GetTickerRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var result = await ExchangeData.GetTickersAsync(request.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTicker>(Exchange, default);

            var ticker = result.Data.Single();
            return result.AsExchangeResult(Exchange, new SharedTicker(ticker.Symbol, ticker.LastPrice, ticker.HighPrice, ticker.LowPrice));
        }

        async Task<ExchangeWebResult<IEnumerable<SharedTicker>>> ITickerRestClient.GetTickersAsync(ApiType? apiType, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedTicker>>(Exchange, default);

            return result.AsExchangeResult<IEnumerable<SharedTicker>>(Exchange, result.Data.Select(x => new SharedTicker(x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice)));
        }

        #endregion

        #region Recent Trade client
        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(1000);

        async Task<ExchangeWebResult<IEnumerable<SharedTrade>>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var result = await ExchangeData.GetTradeHistoryAsync(
                request.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, default);

            return result.AsExchangeResult(Exchange, result.Data.Select(x => new SharedTrade(x.Quantity, x.Price, x.Timestamp)));
        }

        #endregion

        #region Balance client
        EndpointOptions IBalanceRestClient.GetBalancesOptions { get; } = new EndpointOptions(true);

        async Task<ExchangeWebResult<IEnumerable<SharedBalance>>> IBalanceRestClient.GetBalancesAsync(ApiType? apiType, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedBalance>>(Exchange, default);

            return result.AsExchangeResult(Exchange, result.Data.Select(x => new SharedBalance(x.Asset, x.Free, x.Total)));
        }

        #endregion

        #region Spot Order Client

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(
            new[]
            {
                SharedOrderType.Limit,
                SharedOrderType.Market,
                SharedOrderType.LimitMaker
            },
            new[]
            {
                SharedTimeInForce.GoodTillCanceled,
                SharedTimeInForce.ImmediateOrCancel,
            },
            new SharedQuantitySupport(
                SharedQuantityType.Both,
                SharedQuantityType.Both,
                SharedQuantityType.QuoteAssetQuantity,
                SharedQuantityType.BaseAssetQuantity));

        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.PlaceOrderAsync(PlaceSpotOrderRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            if (request.OrderType == SharedOrderType.Other)
                throw new ArgumentException("OrderType can't be `Other`", nameof(request.OrderType));

            var result = await Trading.PlaceOrderAsync(
                request.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                (request.OrderType == SharedOrderType.Limit || request.OrderType == SharedOrderType.LimitMaker) ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: request.Quantity,
                quoteQuantity: request.QuoteQuantity,
                price: request.Price,
                timeInForce: GetTimeInForce(request.OrderType, request.TimeInForce),
                clientOrderId: request.ClientOrderId).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, default);

            return result.AsExchangeResult(Exchange, new SharedId(result.Data.OrderId.ToString()));
        }

        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderRestClient.GetOrderAsync(GetOrderRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, new ArgumentError("Invalid order id"));

            var order = await Trading.GetOrderAsync(request.GetSymbol(FormatSymbol), orderId).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder>(Exchange, default);

            return order.AsExchangeResult(Exchange, new SharedSpotOrder(
                order.Data.Symbol,
                order.Data.OrderId.ToString(),
                ParseOrderType(order.Data.Type, null),
                order.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                Price = order.Data.Price,
                Quantity = order.Data.Quantity,
                QuantityFilled = order.Data.QuantityFilled,
                QuoteQuantity = order.Data.QuoteQuantity,
                QuoteQuantityFilled = order.Data.ValueFilled,
                Fee = order.Data.Fee,
                FeeAsset = order.Data.FeeAsset,
                UpdateTime = order.Data.UpdateTime,
            });
        }

        async Task<ExchangeWebResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetOpenOrdersAsync(GetSpotOpenOrdersRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            string? symbol = null;
            if (request.BaseAsset != null && request.QuoteAsset != null)
                symbol = FormatSymbol(request.BaseAsset, request.QuoteAsset, ApiType.Spot);

            var orders = await Trading.GetOpenOrdersAsync(symbol).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, default);

            return orders.AsExchangeResult(Exchange, orders.Data.Select(x => new SharedSpotOrder(
                x.Symbol,
                x.OrderId.ToString(),
                ParseOrderType(x.Type, null),
                x.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Price = x.Price,
                Quantity = x.Quantity,
                QuantityFilled = x.QuantityFilled,
                QuoteQuantity = x.QuoteQuantity,
                QuoteQuantityFilled = x.ValueFilled,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                UpdateTime = x.UpdateTime,
            }));
        }

        async Task<ExchangeWebResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetClosedOrdersAsync(GetSpotClosedOrdersRequest request, INextPageToken? pageToken, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            // Determine page token
            int page = 1;
            int pageSize = request.Filter?.Limit ?? 500;
            if (pageToken is PageToken token)
            {
                page = token.Page;
                pageSize = token.PageSize;
            }

            // Get data
            var orders = await Trading.GetOrdersAsync(request.GetSymbol(FormatSymbol),
                startTime: request.Filter?.StartTime,
                endTime: request.Filter?.EndTime,
                page: page,
                pageSize: pageSize).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, default);

            // Get next token
            PageToken? nextToken = null;
            if (orders.Data.Count() == pageSize)
                nextToken = new PageToken(page + 1, pageSize);

            return orders.AsExchangeResult(Exchange, orders.Data.Select(x => new SharedSpotOrder(
                x.Symbol,
                x.OrderId.ToString(),
                ParseOrderType(x.Type, null),
                x.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Price = x.Price,
                Quantity = x.Quantity,
                QuantityFilled = x.QuantityFilled,
                QuoteQuantity = x.QuoteQuantity,
                QuoteQuantityFilled = x.ValueFilled,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                UpdateTime = x.UpdateTime,
            }), nextToken);
        }

        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetOrderTradesAsync(GetOrderTradesRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, new ArgumentError("Invalid order id"));

            var orders = await Trading.GetUserTradesAsync(request.GetSymbol(FormatSymbol), orderId: orderId).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, default);

            return orders.AsExchangeResult(Exchange, orders.Data.Select(x => new SharedUserTrade(
                x.Symbol,
                x.OrderId.ToString(),
                x.Id.ToString(),
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Role = x.IsMaker ? SharedRole.Maker : SharedRole.Taker,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
            }));
        }

        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            // Determine page token
            long? fromId = null;
            if (pageToken is FromIdToken fromIdToken)
                fromId = long.Parse(fromIdToken.FromToken);

            // Get data
            var orders = await Trading.GetUserTradesAsync(
                request.GetSymbol(FormatSymbol),
                startTime: request.Filter?.StartTime,
                endTime: request.Filter?.EndTime,
                limit: request.Filter?.Limit ?? 500,
                fromId: fromId).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, default);

            // Get next token
            FromIdToken? nextToken = null;
            if (orders.Data.Count() == (request.Filter?.Limit ?? 500))
                nextToken = new FromIdToken(orders.Data.Max(o => o.Id).ToString());

            return orders.AsExchangeResult(Exchange, orders.Data.Select(x => new SharedUserTrade(
                x.Symbol,
                x.OrderId.ToString(),
                x.Id.ToString(),
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Role = x.IsMaker ? SharedRole.Maker : SharedRole.Taker,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
            }), nextToken);
        }

        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.CancelOrderAsync(CancelOrderRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedId>(Exchange, new ArgumentError("Invalid order id"));

            var order = await Trading.CancelOrderAsync(request.GetSymbol(FormatSymbol), orderId).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, default);

            return order.AsExchangeResult(Exchange, new SharedId(order.Data.OrderId.ToString()));
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

        #region Asset client
        EndpointOptions IAssetRestClient.GetAssetsOptions { get; } = new EndpointOptions(true);

        async Task<ExchangeWebResult<IEnumerable<SharedAsset>>> IAssetRestClient.GetAssetsAsync(ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var result = await Account.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedAsset>>(Exchange, default);

            return result.AsExchangeResult(Exchange, result.Data.Select(x => new SharedAsset(x.Asset)
            {
                FullName = x.Name,
                Networks = x.Networks.Select(x => new SharedAssetNetwork(x.Network)
                {
                    DepositEnabled = x.DepositEnabled,
                    MaxWithdrawQuantity = x.MaxWithdraw,
                    MinConfirmations = x.MinConfirmations,
                    MinWithdrawQuantity = x.MinWithdraw,
                    WithdrawEnabled = x.WithdrawEnabled,
                    WithdrawFee = x.WithdrawFee
                })
            }));
        }

        #endregion

        #region Deposit client

        async Task<ExchangeWebResult<IEnumerable<SharedDepositAddress>>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var depositAddresses = await Account.GetDepositAddressAsync(request.Asset).ConfigureAwait(false);
            if (!depositAddresses)
                return depositAddresses.AsExchangeResult<IEnumerable<SharedDepositAddress>>(Exchange, default);

            var result = depositAddresses.Data.Data;
            if (request.Network != null)
                result = result.Where(r => r.Network == request.Network);

            return depositAddresses.AsExchangeResult(Exchange, result.Select(x => new SharedDepositAddress(x.Asset, x.Address)
            {
                Network = x.Network
            }
            ));
        }

        async Task<ExchangeWebResult<IEnumerable<SharedDeposit>>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, INextPageToken? pageToken, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            // Get data
            var result = await Account.GetDepositHistoryAsync(request.Asset,
                startTime: request.Filter?.StartTime,
                endTime: request.Filter?.EndTime,
                limit: request.Filter?.Limit ?? 100,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedDeposit>>(Exchange, default);

            // Determine next token
            OffsetToken? nextToken = null;
            if (result.Data.Count() == (request.Filter?.Limit ?? 100))
                nextToken = new OffsetToken((offset ?? 0) + result.Data.Count());

            return result.AsExchangeResult(Exchange, result.Data.Select(x => new SharedDeposit(x.Asset, x.Quantity, x.Status == DepositStatus.Completed, x.InsertTime)
            {
                Confirmations = x.ConfirmedTimes.Contains("/") ? int.Parse(x.ConfirmedTimes.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) : null,
                Tag = x.AddressTag,
                TransactionId = x.TransactionId,
                Network = x.Network
            }), nextToken);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 2000);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.Validate(request);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, default);

            return result.AsExchangeResult(Exchange, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }
        #endregion

        #region Withdrawal client

        async Task<ExchangeWebResult<IEnumerable<SharedWithdrawal>>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, INextPageToken? pageToken, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            // Get data
            var withdrawals = await Account.GetWithdrawalHistoryAsync(
                request.Asset,
                startTime: request.Filter?.StartTime,
                endTime: request.Filter?.EndTime,
                limit: request.Filter?.Limit ?? 100,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!withdrawals)
                return withdrawals.AsExchangeResult<IEnumerable<SharedWithdrawal>>(Exchange, default);

            // Determine next token
            OffsetToken nextToken;
            if (withdrawals.Data.Count() == (request.Filter?.Limit ?? 100))
                nextToken = new OffsetToken((offset ?? 0) + withdrawals.Data.Count());

            return withdrawals.AsExchangeResult(Exchange, withdrawals.Data.Select(x => new SharedWithdrawal(x.Asset, x.Address, x.Quantity, x.Status == WithdrawalStatus.Completed, x.ApplyTime)
            {
                Confirmations = x.Confirmations,
                Network = x.Network,
                Tag = x.AddressTag,
                TransactionId = x.TransactionId,
                Fee = x.Fee
            }));
        }

        #endregion

        #region Withdraw client

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions();

        async Task<ExchangeWebResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var validationError = ((IWithdrawRestClient)this).WithdrawOptions.Validate(request);
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
                return withdrawal.AsExchangeResult<SharedId>(Exchange, default);

            return withdrawal.AsExchangeResult(Exchange, new SharedId(withdrawal.Data.Id));
        }

        #endregion
    }
}
