using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Order book info
    /// </summary>
    public record BingXOrderBook
    {
        /// <summary>
        /// List of bids
        /// </summary>
        [JsonPropertyName("bids")]
        public IEnumerable<BingXOrderBookEntry> Bids { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// List of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public IEnumerable<BingXOrderBookEntry> Asks { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public record BingXOrderBookEntry
    {
        /// <summary>
        /// The price
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// The quantity
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }
}
