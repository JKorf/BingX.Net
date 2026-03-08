using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Margin change info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXMarginHistory
    {
        /// <summary>
        /// ["<c>records</c>"] Records
        /// </summary>
        [JsonPropertyName("records")]
        public BingXMarginHistoryEntry[] Records { get; set; } = Array.Empty<BingXMarginHistoryEntry>();
        /// <summary>
        /// ["<c>total</c>"] Total amount of records
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Margin change info
    /// </summary>
    public record BingXMarginHistoryEntry
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>positionId</c>"] Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>changeReason</c>"] Change reason
        /// </summary>
        [JsonPropertyName("changeReason")]
        public string ChangeReason { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>marginChange</c>"] Margin change
        /// </summary>
        [JsonPropertyName("marginChange")]
        public decimal MarginChange { get; set; }
        /// <summary>
        /// ["<c>marginAfterChange</c>"] Margin after change
        /// </summary>
        [JsonPropertyName("marginAfterChange")]
        public decimal MarginAfterChange { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Time of change
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
    }


}
