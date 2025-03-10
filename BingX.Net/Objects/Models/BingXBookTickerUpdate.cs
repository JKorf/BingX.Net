using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Best book prices
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXBookTickerUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonPropertyName("b")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best bid quantity
        /// </summary>
        [JsonPropertyName("B")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonPropertyName("a")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Best ask quantity
        /// </summary>
        [JsonPropertyName("A")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// Last update id
        /// </summary>
        [JsonPropertyName("u")]
        public long? UpdateId { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime? UpdateTime { get; set; }
    }
}
