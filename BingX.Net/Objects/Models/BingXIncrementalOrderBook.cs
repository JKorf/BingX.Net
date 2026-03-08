using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Order book info
    /// </summary>
    public record BingXIncrementalOrderBook
    {
        /// <summary>
        /// ["<c>action</c>"] Action
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        /// <summary>
        /// ["<c>bids</c>"] List of bids
        /// </summary>
        [JsonPropertyName("bids")]
        public Dictionary<decimal, decimal> Bids { get; set; } = new Dictionary<decimal, decimal>();
        /// <summary>
        /// ["<c>asks</c>"] List of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public Dictionary<decimal, decimal> Asks { get; set; } = new Dictionary<decimal, decimal>();
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp of the data
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
        /// <summary>
        /// ["<c>lastUpdateId</c>"] Last update id
        /// </summary>
        [JsonPropertyName("lastUpdateId")]
        public long LastUpdateId { get; set; }
        /// <summary>
        /// ["<c>sourceUpdateId</c>"] Source update id
        /// </summary>
        [JsonPropertyName("sourceUpdateId")]
        public long SourceUpdateId { get; set; }
    }
}
