using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Position information
    /// </summary> 
    [SerializationModel(typeof(BingXResult<>))]
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
        /// Value of the position
        /// </summary>
        [JsonPropertyName("positionValue")]
        public decimal PositionValue { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public TradeSide Side { get; set; }
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
        [JsonPropertyName("margin")]
        public decimal? Margin { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// Maximum margin reduction
        /// </summary>
        [JsonPropertyName("maxMarginReduction")]
        public decimal? MaxMarginReduction { get; set; }
        /// <summary>
        /// Max margin reduction
        /// </summary>
        [JsonPropertyName("onlyOnePosition")]
        public bool? OnlyOnePosition { get; set; }
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
        /// Risk rate
        /// </summary>
        [JsonPropertyName("riskRate")]
        public decimal? RiskRate { get; set; }
        /// <summary>
        /// Liquidation price
        /// </summary>
        [JsonPropertyName("liquidationPrice")]
        public decimal LiquidationPrice { get; set; }
        /// <summary>
        /// Unrealized Profit and loss ratio
        /// </summary>
        [JsonPropertyName("pnlRatio")]
        public decimal UnrealizedPnlRatio { get; set; }
        /// <summary>
        /// Last update timestamp
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }
}
