using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Balance update
    /// </summary>
    [SerializationModel]
    public record BingXBalanceUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Timestamp of the update
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Event info
        /// </summary>
        [JsonPropertyName("a")]
        public BingXBalanceItems EventData { get; set; } = default!;
    }

    /// <summary>
    /// Balance change event data
    /// </summary>
    [SerializationModel]
    public record BingXBalanceItems
    {
        /// <summary>
        /// Changed balances
        /// </summary>
        [JsonPropertyName("B")]
        public BingXBalanceItem[] Balances { get; set; } = Array.Empty<BingXBalanceItem>();
        /// <summary>
        /// The event that caused the update
        /// </summary>
        [JsonPropertyName("m")]
        public string UpdateType { get; set; } = string.Empty;
    }

    /// <summary>
    /// Balance update item
    /// </summary>
    [SerializationModel]
    public record BingXBalanceItem
    {
        /// <summary>
        /// Asset name
        /// </summary>
        [JsonPropertyName("a")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Balance change
        /// </summary>
        [JsonPropertyName("bc")]
        public decimal BalanceChange { get; set; }
        /// <summary>
        /// New balance
        /// </summary>
        [JsonPropertyName("cw")]
        public decimal NewBalance { get; set; }
        /// <summary>
        /// Total balance
        /// </summary>
        [JsonPropertyName("wb")]
        public decimal Total { get; set; }
        /// <summary>
        /// Locked balance
        /// </summary>
        [JsonPropertyName("lk")]
        public decimal Locked { get; set; }
    }
}
