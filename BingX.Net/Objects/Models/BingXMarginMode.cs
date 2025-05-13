using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Margin mode info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXMarginMode
    {
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("marginType")]
        public MarginMode MarginMode { get; set; }
    }
}
