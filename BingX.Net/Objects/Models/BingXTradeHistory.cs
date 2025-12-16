using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    public record BingXTradeHistory
    {
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("tid")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("v")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// timestamp
        /// </summary>
        [JsonPropertyName("t")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;

        [JsonInclude, JsonPropertyName("ms")]
        internal int BuyerIsMakerInt { get; set; }

        /// <summary>
        /// Buyer is maker
        /// </summary>
        public bool BuyerIsMaker => BuyerIsMakerInt == 1;
    }
}
