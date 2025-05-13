using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesBookTickerWrapper
    {
        [JsonPropertyName("book_ticker")]
        public BingXFuturesBookTicker BookTicker { get; set; } = null!;
    }

    /// <summary>
    /// Best offer info
    /// </summary>
    public record BingXFuturesBookTicker
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Current best bid price
        /// </summary>
        [JsonPropertyName("bid_price")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Current best bid quantity
        /// </summary>
        [JsonPropertyName("bid_qty")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Current best ask price
        /// </summary>
        [JsonPropertyName("ask_price")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Current best ask quantity
        /// </summary>
        [JsonPropertyName("ask_qty")]
        public decimal BestAskQuantity { get; set; }
    }
}
