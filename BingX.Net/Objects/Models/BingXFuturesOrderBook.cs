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
    public record BingXFuturesOrderBook
    {
        /// <summary>
        /// List of bids where quantity is in contracts
        /// </summary>
        [JsonPropertyName("bids")]
        public BingXOrderBookEntry[] Bids { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// List of asks where quantity is in contracts
        /// </summary>
        [JsonPropertyName("asks")]
        public BingXOrderBookEntry[] Asks { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// List of bids where quantity is in the quote asset
        /// </summary>
        [JsonPropertyName("bidsCoin")]
        public BingXOrderBookEntry[] BidsAsset { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// List of asks where quantity is in the quote asset
        /// </summary>
        [JsonPropertyName("asksCoin")]
        public BingXOrderBookEntry[] AsksAsset { get; set; } = Array.Empty<BingXOrderBookEntry>();
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime? Timestamp { get; set; }
    }
}
