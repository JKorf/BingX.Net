using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Price update
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXPriceUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The current last traded price
        /// </summary>
        [JsonPropertyName("c")]
        public decimal Price { get; set; }
    }
}
