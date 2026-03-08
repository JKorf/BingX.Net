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
        /// ["<c>s</c>"] Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>b</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("b")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>B</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("B")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>a</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("a")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>A</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("A")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>u</c>"] Last update id
        /// </summary>
        [JsonPropertyName("u")]
        public long? UpdateId { get; set; }
        /// <summary>
        /// ["<c>T</c>"] Last update time
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime? UpdateTime { get; set; }
    }
}
