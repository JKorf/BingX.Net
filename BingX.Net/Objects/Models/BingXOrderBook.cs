using BingX.Net.Converters;
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
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXOrderBook
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        /// <summary>
        /// List of bids
        /// </summary>
        [JsonPropertyName("bids")]
        public BingXOrderBookEntry[] Bids { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// List of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public BingXOrderBookEntry[] Asks { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Futures timestamp
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime? T { set => Timestamp = value; }
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<BingXOrderBookEntry>))]
    [SerializationModel]
    public record BingXOrderBookEntry : ISymbolOrderBookEntry
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
