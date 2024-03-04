using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using BingX.Net.Objects.Models;
using System.Collections.Generic;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBingXRestClientSpotApiExchangeData
    {
        /// <summary>
        /// 
        /// <para><a href="BingX" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXSymbol>>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="limit"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BingXTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);
    }
}
