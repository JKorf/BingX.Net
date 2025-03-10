using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Funding rate info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXFundingRate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Last funding rate
        /// </summary>
        [JsonPropertyName("lastFundingRate")]
        public decimal LastFundingRate { get; set; }
        /// <summary>
        /// Next funding time
        /// </summary>
        [JsonPropertyName("nextFundingTime")]
        public DateTime NextFundingTime { get; set; }
    }
}
