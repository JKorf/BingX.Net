using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using CryptoExchange.Net.Objects;
using System.Threading.Tasks;
using System.Threading;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX Spot trading endpoints, placing and managing orders.
    /// </summary>
    public interface IBingXRestClientSpotApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Create%20an%20Order" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="side">Order side</param>
        /// <param name="type">Order type</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="price">Order price for limit orders</param>
        /// <param name="quoteQuantity">Order quantity in quote asset</param>
        /// <param name="stopPrice">Stop price</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, decimal? stopPrice = null, string? clientOrderId = null, CancellationToken ct = default);
    }
}
