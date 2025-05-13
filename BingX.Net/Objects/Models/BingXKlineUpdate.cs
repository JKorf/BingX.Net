using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Kline (candlestick) update
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXKlineUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Kline data
        /// </summary>
        [JsonPropertyName("K")]
        public BingXKlineUpdateData Kline { get; set; } = null!;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
    }

    /// <summary>
    /// Kline (candlestick) info
    /// </summary>
    public record BingXKlineUpdateData
    {
        /// <summary>
        /// Open timestamp
        /// </summary>
        [JsonPropertyName("t")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("o")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("h")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("l")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Close price
        /// </summary>
        [JsonPropertyName("c")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Close time
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime CloseTime { get; set; }
        /// <summary>
        /// Quote volume
        /// </summary>
        [JsonPropertyName("q")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Number of trades
        /// </summary>
        [JsonPropertyName("n")]
        public int TradeCount { get; set; }
    }
}
