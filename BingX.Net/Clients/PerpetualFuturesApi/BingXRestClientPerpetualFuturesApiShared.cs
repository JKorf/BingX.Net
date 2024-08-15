using BingX.Net;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis.Interfaces;
using CryptoExchange.Net.SharedApis.Models;
using CryptoExchange.Net.SharedApis.Models.Rest;
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

        async Task<ExchangeWebResult<IEnumerable<SharedKline>>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, new ArgumentError("Interval not supported"));

            var result = await ExchangeData.GetKlinesAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType),
                interval,
                request.StartTime,
                request.EndTime,
                request.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedKline>>(Exchange, default);

            return result.AsExchangeResult(Exchange, result.Data.Select(x => new SharedKline(x.Timestamp, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)));
        }

        async Task<ExchangeWebResult<IEnumerable<SharedFuturesSymbol>>> IFuturesSymbolRestClient.GetSymbolsAsync(SharedRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetContractsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedFuturesSymbol>>(Exchange, default);

            return result.AsExchangeResult(Exchange, result.Data.Select(s => new SharedFuturesSymbol(s.Asset, s.Currency, s.Symbol)
            {
                MinTradeQuantity = s.MinOrderQuantity,
                PriceDecimals = s.PricePrecision,
                QuantityDecimals = s.QuantityPrecision,
                ContractSize = s.ContractSize
            }));
        }

        async Task<ExchangeWebResult<SharedTicker>> ITickerRestClient.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetTickerAsync(FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType), ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTicker>(Exchange, default);

            return result.AsExchangeResult(Exchange, new SharedTicker(result.Data.Symbol, result.Data.LastPrice, result.Data.HighPrice, result.Data.LowPrice));
        }

        async Task<ExchangeWebResult<IEnumerable<SharedTicker>>> ITickerRestClient.GetTickersAsync(SharedRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedTicker>>(Exchange, default);

            return result.AsExchangeResult<IEnumerable<SharedTicker>>(Exchange, result.Data.Select(x => new SharedTicker(x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice)));
        }

        async Task<ExchangeWebResult<IEnumerable<SharedTrade>>> ITradeRestClient.GetTradesAsync(GetTradesRequest request, CancellationToken ct)
        {
            if (request.StartTime != null || request.EndTime != null)
                return new ExchangeWebResult<IEnumerable<SharedTrade>>(Exchange, new ArgumentError("Start/EndTime filtering not supported"));

            var result = await ExchangeData.GetRecentTradesAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, default);

            return result.AsExchangeResult(Exchange, result.Data.Select(x => new SharedTrade(x.Quantity, x.Price, x.Timestamp)));
        }
    }
}
