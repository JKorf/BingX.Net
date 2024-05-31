using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Position information
    /// </summary>
    public record BingXPosition
    {
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Position size
        /// </summary>
        [JsonPropertyName("positionAmt")]
        public decimal Size { get; set; }
        /// <summary>
        /// Availalbe quantity
        /// </summary>
        [JsonPropertyName("availableAmt")]
        public decimal Available { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide Side { get; set; }
        /// <summary>
        /// Is isolated margin mode
        /// </summary>
        [JsonPropertyName("isolated")]
        public bool Isolated { get; set; }
        /// <summary>
        /// Average open price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// Initial margin
        /// </summary>
        [JsonPropertyName("initialMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("Margin")]
        public decimal? Margin { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Unrealized profit
        /// </summary>
        [JsonPropertyName("unrealizedProfit")]
        public decimal UnrealizedProfit { get; set; }
        /// <summary>
        /// Realized profit
        /// </summary>
        [JsonPropertyName("realisedProfit")]
        public decimal RealizedProfit { get; set; }
        /// <summary>
        /// Liquidation price
        /// </summary>
        [JsonPropertyName("liquidationPrice")]
        public decimal LiquidationPrice { get; set; }
        /// <summary>
        /// Last update timestamp
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }
}
