using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Set isolated margin result 
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXIsolatedMarginResult
    {
        /// <summary>
        /// ["<c>amount</c>"] Adjusted quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Direction
        /// </summary>
        [JsonPropertyName("type")]
        public AdjustDirection Direction { get; set; }
    }
}
