using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Position mode
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXPositionMode
    {
        /// <summary>
        /// Position mode
        /// </summary>
        [JsonPropertyName("dualSidePosition")]
        public PositionMode PositionMode { get; set; }
    }
}
