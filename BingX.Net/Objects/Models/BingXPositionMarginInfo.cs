using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Position and maintenance margin ratio info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXPositionMarginInfo
    {
        /// <summary>
        /// ["<c>tier</c>"] Tier
        /// </summary>
        [JsonPropertyName("tier")]
        public string Tier { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>minPositionVal</c>"] Minimal position value
        /// </summary>
        [JsonPropertyName("minPositionVal")]
        public decimal MinPositionValue { get; set; }
        /// <summary>
        /// ["<c>maxPositionVal</c>"] Max position value
        /// </summary>
        [JsonPropertyName("maxPositionVal")]
        public decimal MaxPositionValue { get; set; }
        /// <summary>
        /// ["<c>maintMarginRatio</c>"] Maintenance margin ratio
        /// </summary>
        [JsonPropertyName("maintMarginRatio")]
        public decimal MaintenanceMarginRatio { get; set; }
        /// <summary>
        /// ["<c>maintAmount</c>"] Maintenance amount
        /// </summary>
        [JsonPropertyName("maintAmount")]
        public decimal MaintenanceAmount { get; set; }
    }
}
