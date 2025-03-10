using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Records
        /// </summary>
        [JsonPropertyName("records")]
        public BingXMarginHistoryEntry[] Records { get; set; } = Array.Empty<BingXMarginHistoryEntry>();
        /// <summary>
        /// Total amount of records
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
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// Change reason
        /// </summary>
        [JsonPropertyName("changeReason")]
        public string ChangeReason { get; set; } = string.Empty;
        /// <summary>
        /// Margin change
        /// </summary>
        [JsonPropertyName("marginChange")]
        public decimal MarginChange { get; set; }
        /// <summary>
        /// Margin after change
        /// </summary>
        [JsonPropertyName("marginAfterChange")]
        public decimal MarginAfterChange { get; set; }
        /// <summary>
        /// Time of change
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
    }


}
