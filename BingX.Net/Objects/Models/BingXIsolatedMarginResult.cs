using BingX.Net.Enums;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Set isolated margin result 
    /// </summary>
    public record BingXIsolatedMarginResult
    {
        /// <summary>
        /// Adjusted quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Direction
        /// </summary>
        [JsonPropertyName("type")]
        public AdjustDirection Direction { get; set; }
    }
}
