using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis.Enums;
using CryptoExchange.Net.SharedApis.Interfaces.Socket;
using CryptoExchange.Net.SharedApis.Models;
using CryptoExchange.Net.SharedApis.Models.FilterOptions;
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

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXSocketClientPerpetualFuturesApi : IBingXSocketClientPerpetualFuturesApiShared
    {
        public string Exchange => BingXExchange.ExchangeName;

        public ApiType[] SupportedApiTypes { get; } = new[] { ApiType.PerpetualLinear };

        #region Ticker client
        SubscriptionOptions<SubscribeTickerRequest> ITickerSocketClient.SubscribeTickerOptions { get; } = new SubscriptionOptions<SubscribeTickerRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<ExchangeEvent<SharedSpotTicker>> handler, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var validationError = ((ITickerSocketClient)this).SubscribeTickerOptions.ValidateRequest(Exchange, request, exchangeParameters, request.ApiType, SupportedApiTypes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol((baseAsset, quoteAsset) => FormatSymbol(baseAsset, quoteAsset, request.ApiType));
            var result = await SubscribeToTickerUpdatesAsync(symbol, update => handler(update.AsExchangeEvent(Exchange, new SharedSpotTicker(update.Data.Symbol, update.Data.HighPrice, update.Data.LastPrice, update.Data.LowPrice, update.Data.Volume))), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Trade client

        SubscriptionOptions<SubscribeTradeRequest> ITradeSocketClient.SubscribeTradeOptions { get; } = new SubscriptionOptions<SubscribeTradeRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<ExchangeEvent<IEnumerable<SharedTrade>>> handler, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var validationError = ((ITradeSocketClient)this).SubscribeTradeOptions.ValidateRequest(Exchange, request, exchangeParameters, request.ApiType, SupportedApiTypes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol((baseAsset, quoteAsset) => FormatSymbol(baseAsset, quoteAsset, request.ApiType));
            var result = await SubscribeToTradeUpdatesAsync(symbol, update => handler(update.AsExchangeEvent<IEnumerable<SharedTrade>>(Exchange, update.Data.Select(x => new SharedTrade(x.Price, x.Quantity, x.TradeTime)))), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Book Ticker client

        SubscriptionOptions<SubscribeBookTickerRequest> IBookTickerSocketClient.SubscribeBookTickerOptions { get; } = new SubscriptionOptions<SubscribeBookTickerRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<ExchangeEvent<SharedBookTicker>> handler, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var validationError = ((IBookTickerSocketClient)this).SubscribeBookTickerOptions.ValidateRequest(Exchange, request, exchangeParameters, request.ApiType, SupportedApiTypes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol((baseAsset, quoteAsset) => FormatSymbol(baseAsset, quoteAsset, request.ApiType));
            var result = await SubscribeToBookPriceUpdatesAsync(symbol, update => handler(update.AsExchangeEvent(Exchange, new SharedBookTicker(update.Data.BestAskPrice, update.Data.BestAskQuantity, update.Data.BestBidPrice, update.Data.BestBidQuantity))), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Balance client
        SubscriptionOptions IBalanceSocketClient.SubscribeBalanceOptions { get; } = new SubscriptionOptions("SubscribeBalanceRequest", false)
        {
            RequiredExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription("ListenKey", typeof(string), "The listenkey for starting the user stream", "123123123")
            }
        };
        async Task<ExchangeResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(ApiType apiType, Action<ExchangeEvent<IEnumerable<SharedBalance>>> handler, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var validationError = ((IBalanceSocketClient)this).SubscribeBalanceOptions.ValidateRequest(Exchange, exchangeParameters, apiType, SupportedApiTypes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var listenKey = exchangeParameters.GetValue<string>(Exchange, "ListenKey");

            var result = await SubscribeToUserDataUpdatesAsync(listenKey,
                onAccountUpdate: update => handler(update.AsExchangeEvent(Exchange, update.Data.Update.Balances.Select(x => new SharedBalance(x.Asset, x.Balance, x.Balance)))),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Futures Order client
        SubscriptionOptions IFuturesOrderSocketClient.SubscribeFuturesOrderOptions { get; } = new SubscriptionOptions("SubscribeSpotOrderRequest", false)
        {
            RequiredExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription("ListenKey", typeof(string), "The listenkey for starting the user stream", "123123123")
            }
        };
        async Task<ExchangeResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(ApiType apiType, Action<ExchangeEvent<IEnumerable<SharedFuturesOrder>>> handler, ExchangeParameters? exchangeParameters, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderSocketClient)this).SubscribeFuturesOrderOptions.ValidateRequest(Exchange, exchangeParameters, apiType, SupportedApiTypes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var listenKey = exchangeParameters.GetValue<string>(Exchange, "ListenKey");
            var result = await SubscribeToUserDataUpdatesAsync(listenKey,
                onOrderUpdate: update => handler(update.AsExchangeEvent<IEnumerable<SharedFuturesOrder>>(Exchange, new[] {
                    new SharedFuturesOrder(
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        update.Data.Type == Enums.FuturesOrderType.Limit ? SharedOrderType.Limit : update.Data.Type == Enums.FuturesOrderType.Market ? SharedOrderType.Market : SharedOrderType.Other,
                        update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        update.Data.Status == Enums.OrderStatus.Canceled ? SharedOrderStatus.Canceled : (update.Data.Status == Enums.OrderStatus.New || update.Data.Status == Enums.OrderStatus.PartiallyFilled) ? SharedOrderStatus.Open : SharedOrderStatus.Filled,
                        update.Data.UpdateTime)
                    {
                        ClientOrderId = update.Data.ClientOrderId,
                        Price = update.Data.Price,
                        Quantity = update.Data.Quantity,
                        QuantityFilled = update.Data.QuantityFilled,
                        QuoteQuantityFilled = update.Data.VolumeFilled,
                        Fee = update.Data.Fee,
                        AveragePrice = update.Data.AveragePrice,
                        PositionSide = update.Data.PositionSide == Enums.PositionSide.Long ? SharedPositionSide.Long : update.Data.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : null,
                        FeeAsset = update.Data.FeeAsset,
                        UpdateTime = update.Data.UpdateTime
                    }
                })),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

    }
}
