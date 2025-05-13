using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Ticker information over last 24h
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXTickerUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Price change
        /// </summary>
        [JsonPropertyName("p")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// Price change percentage
        /// </summary>
        [JsonPropertyName("P")]
        public string PriceChangePercentage { get; set; } = string.Empty;
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
        /// Last trade price
        /// </summary>
        [JsonPropertyName("c")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Quote volume
        /// </summary>
        [JsonPropertyName("q")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Open time
        /// </summary>
        [JsonPropertyName("O")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Close time
        /// </summary>
        [JsonPropertyName("C")]
        public DateTime CloseTime { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonPropertyName("B")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best bid quantity
        /// </summary>
        [JsonPropertyName("b")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonPropertyName("A")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Best ask quantity
        /// </summary>
        [JsonPropertyName("a")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// Trade count
        /// </summary>
        [JsonPropertyName("n")]
        public int TradeCount { get; set; }
    }
}
