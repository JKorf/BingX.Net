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
        /// ["<c>tid</c>"] Trade id
        /// </summary>
        [JsonPropertyName("tid")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>p</c>"] Trade price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>v</c>"] Quantity
        /// </summary>
        [JsonPropertyName("v")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>t</c>"] timestamp
        /// </summary>
        [JsonPropertyName("t")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>s</c>"] Symbol
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
