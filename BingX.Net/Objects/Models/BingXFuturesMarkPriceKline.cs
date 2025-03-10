using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Mark price kline info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXFuturesMarkPriceKline
    {
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Close price
        /// </summary>
        [JsonPropertyName("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
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
    }
}
