using Newtonsoft.Json;
using System;

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
        public long Id { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// timestamp
        /// </summary>
        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Whether buyer was the maker
        /// </summary>
        [JsonProperty("buyerMaker")]
        public bool BuyerIsMaker { get; set; }
    }
}
