using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>T</c>"] Timestamp of the update
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>a</c>"] Event info
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
        /// ["<c>B</c>"] Changed balances
        /// </summary>
        [JsonPropertyName("B")]
        public BingXBalanceItem[] Balances { get; set; } = Array.Empty<BingXBalanceItem>();
        /// <summary>
        /// ["<c>m</c>"] The event that caused the update
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
        /// ["<c>a</c>"] Asset name
        /// </summary>
        [JsonPropertyName("a")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>bc</c>"] Balance change
        /// </summary>
        [JsonPropertyName("bc")]
        public decimal BalanceChange { get; set; }
        /// <summary>
        /// ["<c>cw</c>"] New balance
        /// </summary>
        [JsonPropertyName("cw")]
        public decimal NewBalance { get; set; }
        /// <summary>
        /// ["<c>wb</c>"] Total balance
        /// </summary>
        [JsonPropertyName("wb")]
        public decimal Total { get; set; }
        /// <summary>
        /// ["<c>lk</c>"] Locked balance
        /// </summary>
        [JsonPropertyName("lk")]
        public decimal Locked { get; set; }
    }
}
