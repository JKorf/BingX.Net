using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>bid_price</c>"] Current best bid price
        /// </summary>
        [JsonPropertyName("bid_price")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bid_qty</c>"] Current best bid quantity
        /// </summary>
        [JsonPropertyName("bid_qty")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>ask_price</c>"] Current best ask price
        /// </summary>
        [JsonPropertyName("ask_price")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>ask_qty</c>"] Current best ask quantity
        /// </summary>
        [JsonPropertyName("ask_qty")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>lastUpdateId</c>"] Last update sequence number
        /// </summary>
        [JsonPropertyName("lastUpdateId")]
        public long LastUpdateId { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Update time
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime UpdateTime { get; set; }
    }
}
