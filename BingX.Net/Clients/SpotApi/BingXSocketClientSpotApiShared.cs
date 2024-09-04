using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis.Enums;
using CryptoExchange.Net.SharedApis.Interfaces.Socket;
using CryptoExchange.Net.SharedApis.Models;
using CryptoExchange.Net.SharedApis.Models.Socket;
using CryptoExchange.Net.SharedApis.RequestModels;
using CryptoExchange.Net.SharedApis.ResponseModels;
using CryptoExchange.Net.SharedApis.SubscribeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXSocketClientSpotApi : IBingXSocketClientSpotApiShared
    {
        public string Exchange => BingXExchange.ExchangeName;

        public ApiType[] SupportedApiTypes { get; } = new[] { ApiType.Spot }; 

        async Task<ExchangeResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(TickerSubscribeRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var symbol = FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType);
            var result = await SubscribeToTickerUpdatesAsync(symbol, update => handler(update.As(new SharedSpotTicker(update.Data.Symbol, update.Data.HighPrice, update.Data.LastPrice, update.Data.LowPrice, update.Data.Volume))), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        async Task<ExchangeResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(TradeSubscribeRequest request, Action<DataEvent<IEnumerable<SharedTrade>>> handler, CancellationToken ct)
        {
            var symbol = FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType);
            var result = await SubscribeToTradeUpdatesAsync(symbol, update => handler(update.As<IEnumerable<SharedTrade>>(new[] { new SharedTrade(update.Data.Price, update.Data.Quantity, update.Data.TradeTime) })), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        async Task<ExchangeResult<UpdateSubscription>> IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(BookTickerSubscribeRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var symbol = FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType);
            var result = await SubscribeToBookPriceUpdatesAsync(symbol, update => handler(update.As(new SharedBookTicker(update.Data.BestAskPrice, update.Data.BestAskQuantity, update.Data.BestBidPrice, update.Data.BestBidQuantity))), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        async Task<ExchangeResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(ApiType? apiType, Action<DataEvent<IEnumerable<SharedBalance>>> handler, CancellationToken ct)
        {
            var restClient = new BingXRestClient(opts =>
            {
                opts.ApiCredentials = ApiOptions.ApiCredentials ?? ClientOptions.ApiCredentials;
                opts.Environment = ((BingXSocketOptions)ClientOptions).Environment;
            });

            var listenKey = await restClient.SpotApi.Account.StartUserStreamAsync().ConfigureAwait(false);
            if (!listenKey)
                return new ExchangeResult<UpdateSubscription>(Exchange, listenKey.As<UpdateSubscription>(default));

            var result = await SubscribeToBalanceUpdatesAsync(listenKey.Data,
                update => handler(update.As(update.Data.EventData.Balances.Select(x => new SharedBalance(x.Asset, x.Total - x.Locked, x.Total)))),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        async Task<ExchangeResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(Action<DataEvent<IEnumerable<SharedSpotOrder>>> handler, CancellationToken ct)
        {
            var restClient = new BingXRestClient(opts =>
            {
                opts.ApiCredentials = ApiOptions.ApiCredentials ?? ClientOptions.ApiCredentials;
                opts.Environment = ((BingXSocketOptions)ClientOptions).Environment;
            });

            var listenKey = await restClient.SpotApi.Account.StartUserStreamAsync().ConfigureAwait(false);
            if (!listenKey)
                return new ExchangeResult<UpdateSubscription>(Exchange, listenKey.As<UpdateSubscription>(default));

            var result = await SubscribeToOrderUpdatesAsync(listenKey.Data,
                update => handler(update.As<IEnumerable<SharedSpotOrder>>(new[] {
                    new SharedSpotOrder(
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        update.Data.Type == Enums.OrderType.Limit ? SharedOrderType.Limit : update.Data.Type == Enums.OrderType.Market ? SharedOrderType.Market : SharedOrderType.Other,
                        update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        update.Data.Status == Enums.OrderStatus.Canceled ? SharedOrderStatus.Canceled : (update.Data.Status == Enums.OrderStatus.New || update.Data.Status == Enums.OrderStatus.PartiallyFilled) ? SharedOrderStatus.Open : SharedOrderStatus.Filled,
                        update.Data.CreateTime)
                    {
                        ClientOrderId = update.Data.ClientOrderId,
                        Price = update.Data.Price,
                        Quantity = update.Data.Quantity,
                        QuantityFilled = update.Data.QuantityFilled,
                        QuoteQuantity = update.Data.QuoteOrderQuantity,
                        QuoteQuantityFilled = update.Data.VolumeFilled,
                        Fee = update.Data.Fee,
                        FeeAsset = update.Data.FeeAsset,
                        UpdateTime = update.Data.UpdateTime,
                        LastTrade = update.Data.LastFillQuantity > 0 ? null : new SharedUserTrade(update.Data.Symbol, update.Data.OrderId.ToString(), update.Data.TradeId.ToString(), update.Data.LastFillQuantity!.Value, update.Data.LastFillPrice!.Value, update.Data.UpdateTime!.Value)
                    }
                })),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
    }
}
