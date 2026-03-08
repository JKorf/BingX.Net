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
        /// ["<c>positionId</c>"] Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>positionAmt</c>"] Position size
        /// </summary>
        [JsonPropertyName("positionAmt")]
        public decimal Size { get; set; }
        /// <summary>
        /// ["<c>availableAmt</c>"] Availalbe quantity
        /// </summary>
        [JsonPropertyName("availableAmt")]
        public decimal Available { get; set; }
        /// <summary>
        /// ["<c>positionValue</c>"] Value of the position
        /// </summary>
        [JsonPropertyName("positionValue")]
        public decimal PositionValue { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public TradeSide Side { get; set; }
        /// <summary>
        /// ["<c>isolated</c>"] Is isolated margin mode
        /// </summary>
        [JsonPropertyName("isolated")]
        public bool Isolated { get; set; }
        /// <summary>
        /// ["<c>avgPrice</c>"] Average open price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// ["<c>initialMargin</c>"] Initial margin
        /// </summary>
        [JsonPropertyName("initialMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal? Margin { get; set; }
        /// <summary>
        /// ["<c>markPrice</c>"] Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// ["<c>maxMarginReduction</c>"] Maximum margin reduction
        /// </summary>
        [JsonPropertyName("maxMarginReduction")]
        public decimal? MaxMarginReduction { get; set; }
        /// <summary>
        /// ["<c>onlyOnePosition</c>"] Max margin reduction
        /// </summary>
        [JsonPropertyName("onlyOnePosition")]
        public bool? OnlyOnePosition { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>unrealizedProfit</c>"] Unrealized profit
        /// </summary>
        [JsonPropertyName("unrealizedProfit")]
        public decimal UnrealizedProfit { get; set; }
        /// <summary>
        /// ["<c>realisedProfit</c>"] Realized profit
        /// </summary>
        [JsonPropertyName("realisedProfit")]
        public decimal RealizedProfit { get; set; }
        /// <summary>
        /// ["<c>riskRate</c>"] Risk rate
        /// </summary>
        [JsonPropertyName("riskRate")]
        public decimal? RiskRate { get; set; }
        /// <summary>
        /// ["<c>liquidationPrice</c>"] Liquidation price
        /// </summary>
        [JsonPropertyName("liquidationPrice")]
        public decimal LiquidationPrice { get; set; }
        /// <summary>
        /// ["<c>pnlRatio</c>"] Unrealized Profit and loss ratio
        /// </summary>
        [JsonPropertyName("pnlRatio")]
        public decimal UnrealizedPnlRatio { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Last update timestamp
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }
}
