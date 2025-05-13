using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Funding rate history 
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXFundingRateHistory
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }
        /// <summary>
        /// Funding time
        /// </summary>
        [JsonPropertyName("fundingTime")]
        public DateTime FundingTime { get; set; }
    }
}
