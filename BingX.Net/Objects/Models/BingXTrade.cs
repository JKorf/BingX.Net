using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    public record BingXTrade
    {
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("qty")]
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Whether buyer was the maker
        /// </summary>
        [JsonProperty("buyerMaker")]
        [JsonPropertyName("buyerMaker")]
        public bool BuyerIsMaker { get; set; }
    }
}
