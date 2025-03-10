using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Mark price update
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXMarkPriceUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The current mark price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal MarkPrice { get; set; }
    }
}
