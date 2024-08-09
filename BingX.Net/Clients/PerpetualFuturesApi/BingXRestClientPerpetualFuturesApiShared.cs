using BingX.Net;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis.Interfaces;
using CryptoExchange.Net.SharedApis.RequestModels;
using CryptoExchange.Net.SharedApis.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXRestClientPerpetualFuturesApi : IBingXRestClientPerpetualFuturesApiShared
    {
        public string Exchange => BingXExchange.ExchangeName;

        async Task<WebCallResult<IEnumerable<SharedKline>>> IKlineClient.GetKlinesAsync(KlineRequest request, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval.TotalSeconds;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new WebCallResult<IEnumerable<SharedKline>>(new ArgumentError("Interval not supported"));

            var result = await ExchangeData.GetKlinesAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.FuturesType),
                interval,
                request.StartTime,
                request.EndTime,
                request.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedKline>>(default);

            return result.As(result.Data.Select(x => new SharedKline
            {
                BaseVolume = x.Volume,
                ClosePrice = x.ClosePrice,
                HighPrice = x.HighPrice,
                LowPrice = x.LowPrice,
                OpenPrice = x.OpenPrice,
                OpenTime = x.Timestamp
            }));
        }

        async Task<WebCallResult<IEnumerable<SharedFuturesSymbol>>> IFuturesSymbolClient.GetSymbolsAsync(SharedRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetContractsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedFuturesSymbol>>(default);

            return result.As(result.Data.Select(s => new SharedFuturesSymbol
            {
                BaseAsset = s.Asset,
                QuoteAsset = s.Currency,
                Name = s.Symbol,
                MinTradeQuantity = s.MinOrderQuantity,
                PriceDecimals = s.PricePrecision,
                QuantityDecimals = s.QuantityPrecision,
                ContractSize = s.ContractSize
            }));
        }

        async Task<WebCallResult<SharedTicker>> ITickerClient.GetTickerAsync(TickerRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetTickerAsync(FormatSymbol(request.BaseAsset, request.QuoteAsset, request.FuturesType), ct).ConfigureAwait(false);
            if (!result)
                return result.As<SharedTicker>(default);

            return result.As(new SharedTicker
            {
                HighPrice = result.Data.HighPrice,
                LastPrice = result.Data.LastPrice,
                LowPrice = result.Data.LowPrice,
            });
        }

        async Task<WebCallResult<IEnumerable<SharedTrade>>> ITradeClient.GetTradesAsync(TradeRequest request, CancellationToken ct)
        {
            if (request.StartTime != null || request.EndTime != null)
                return new WebCallResult<IEnumerable<SharedTrade>>(new ArgumentError("Start/EndTime filtering not supported"));

            var result = await ExchangeData.GetRecentTradesAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.FuturesType),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedTrade>>(default);

            return result.As(result.Data.Select(x => new SharedTrade
            {
                Price = x.Price,
                Quantity = x.Quantity,
                Timestamp = x.Timestamp
            }));
        }
    }
}
