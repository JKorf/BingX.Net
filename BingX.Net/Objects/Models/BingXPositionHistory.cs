using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using BingX.Net.Enums;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXPositionHistoryWrapper
    {
        [JsonPropertyName("positionHistory")]
        public BingXPositionHistory[] History { get; set; } = Array.Empty<BingXPositionHistory>();
    }

    /// <summary>
    /// Position history
    /// </summary>
    public record BingXPositionHistory
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
        /// Isolated (true) or Cross (false) margin mode
        /// </summary>
        [JsonPropertyName("isolated")]
        public bool Isolated { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Open time
        /// </summary>
        [JsonPropertyName("openTime")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Average price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// Average close price
        /// </summary>
        [JsonPropertyName("avgClosePrice")]
        public decimal AverageClosePrice { get; set; }
        /// <summary>
        /// Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedProfit")]
        public decimal RealisedPnl { get; set; }
        /// <summary>
        /// Net profit and loss
        /// </summary>
        [JsonPropertyName("netProfit")]
        public decimal NetPnl { get; set; }
        /// <summary>
        /// Position quantity
        /// </summary>
        [JsonPropertyName("positionAmt")]
        public decimal PositionQuantity { get; set; }
        /// <summary>
        /// Close position quantity
        /// </summary>
        [JsonPropertyName("closePositionAmt")]
        public decimal ClosePositionQuantity { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Are all positions closed
        /// </summary>
        [JsonPropertyName("closeAllPositions")]
        public bool CloseAllPositions { get; set; }
        /// <summary>
        /// Position fee
        /// </summary>
        [JsonPropertyName("positionCommission")]
        public decimal PositionFee { get; set; }
        /// <summary>
        /// Total funding
        /// </summary>
        [JsonPropertyName("totalFunding")]
        public decimal TotalFunding { get; set; }
    }
}
