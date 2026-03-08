using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>isolated</c>"] Isolated (true) or Cross (false) margin mode
        /// </summary>
        [JsonPropertyName("isolated")]
        public bool Isolated { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>openTime</c>"] Open time
        /// </summary>
        [JsonPropertyName("openTime")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>avgPrice</c>"] Average price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// ["<c>avgClosePrice</c>"] Average close price
        /// </summary>
        [JsonPropertyName("avgClosePrice")]
        public decimal AverageClosePrice { get; set; }
        /// <summary>
        /// ["<c>realisedProfit</c>"] Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedProfit")]
        public decimal RealisedPnl { get; set; }
        /// <summary>
        /// ["<c>netProfit</c>"] Net profit and loss
        /// </summary>
        [JsonPropertyName("netProfit")]
        public decimal NetPnl { get; set; }
        /// <summary>
        /// ["<c>positionAmt</c>"] Position quantity
        /// </summary>
        [JsonPropertyName("positionAmt")]
        public decimal PositionQuantity { get; set; }
        /// <summary>
        /// ["<c>closePositionAmt</c>"] Close position quantity
        /// </summary>
        [JsonPropertyName("closePositionAmt")]
        public decimal ClosePositionQuantity { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// ["<c>closeAllPositions</c>"] Are all positions closed
        /// </summary>
        [JsonPropertyName("closeAllPositions")]
        public bool CloseAllPositions { get; set; }
        /// <summary>
        /// ["<c>positionCommission</c>"] Position fee
        /// </summary>
        [JsonPropertyName("positionCommission")]
        public decimal PositionFee { get; set; }
        /// <summary>
        /// ["<c>totalFunding</c>"] Total funding
        /// </summary>
        [JsonPropertyName("totalFunding")]
        public decimal TotalFunding { get; set; }
    }
}
