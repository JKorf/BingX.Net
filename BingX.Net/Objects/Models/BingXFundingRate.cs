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
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>markPrice</c>"] Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// ["<c>indexPrice</c>"] Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// ["<c>lastFundingRate</c>"] Last funding rate
        /// </summary>
        [JsonPropertyName("lastFundingRate")]
        public decimal LastFundingRate { get; set; }
        /// <summary>
        /// ["<c>nextFundingTime</c>"] Next funding time
        /// </summary>
        [JsonPropertyName("nextFundingTime")]
        public DateTime NextFundingTime { get; set; }
        /// <summary>
        /// ["<c>fundingIntervalHours</c>"] Funding interval in hours
        /// </summary>
        [JsonPropertyName("fundingIntervalHours")]
        public int? FundingIntervalHours { get; set; }
        /// <summary>
        /// ["<c>minFundingRate</c>"] Min funding rate
        /// </summary>
        [JsonPropertyName("minFundingRate")]
        public decimal? MinFundingRate { get; set; }
        /// <summary>
        /// ["<c>maxFundingRate</c>"] Max funding rate
        /// </summary>
        [JsonPropertyName("maxFundingRate")]
        public decimal? MaxFundingRate { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }
}
