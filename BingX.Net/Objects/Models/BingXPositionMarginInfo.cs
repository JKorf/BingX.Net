using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Tier
        /// </summary>
        [JsonPropertyName("tier")]
        public string Tier { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Minimal position value
        /// </summary>
        [JsonPropertyName("minPositionVal")]
        public decimal MinPositionValue { get; set; }
        /// <summary>
        /// Max position value
        /// </summary>
        [JsonPropertyName("maxPositionVal")]
        public decimal MaxPositionValue { get; set; }
        /// <summary>
        /// Maintenance margin ratio
        /// </summary>
        [JsonPropertyName("maintMarginRatio")]
        public decimal MaintenanceMarginRatio { get; set; }
        /// <summary>
        /// Maintenance amount
        /// </summary>
        [JsonPropertyName("maintAmount")]
        public decimal MaintenanceAmount { get; set; }
    }
}
