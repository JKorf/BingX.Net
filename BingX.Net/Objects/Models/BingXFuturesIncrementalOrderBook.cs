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
    public record BingXFuturesIncrementalOrderBook
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
        public BingXOrderBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// ["<c>asks</c>"] List of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public BingXOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>time</c>"] Timestamp of the data
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
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
