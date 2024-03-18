using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// BingX futures exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBingXRestClientFuturesApiExchangeData
    {
        /// <summary>
        /// 
        /// <para><a href="BingX" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);
    }
}
