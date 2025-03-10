using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// 24h Price statistics
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXFuturesTicker
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("openPrice")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("highPrice")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("lowPrice")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Last quantity
        /// </summary>
        [JsonPropertyName("lastQty")]
        public decimal LastQuantity { get; set; }
        /// <summary>
        /// Price change
        /// </summary>
        [JsonPropertyName("priceChange")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// Price change percentage
        /// </summary>
        [JsonPropertyName("priceChangePercent")]
        public decimal PriceChangePercent { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Quote volume
        /// </summary>
        [JsonPropertyName("quoteVolume")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Open time
        /// </summary>
        [JsonPropertyName("openTime")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Close time
        /// </summary>
        [JsonPropertyName("closeTime")]
        public DateTime CloseTime { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonPropertyName("askPrice")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Best ask quantity
        /// </summary>
        [JsonPropertyName("askQty")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonPropertyName("bidPrice")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best bid quantity
        /// </summary>
        [JsonPropertyName("bidQty")]
        public decimal BestBidQuantity { get; set; }
    }
}
