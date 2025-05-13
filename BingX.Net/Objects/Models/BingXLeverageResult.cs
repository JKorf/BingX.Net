using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Set leverage result
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXLeverageResult
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Available Long Volume
        /// </summary>
        [JsonPropertyName("availableLongVol")]
        public decimal? AvailableLongVolume { get; set; }
        /// <summary>
        /// Available Short Volume
        /// </summary>
        [JsonPropertyName("availableShortVol")]
        public decimal? AvailableShortVolume { get; set; }
        /// <summary>
        /// Available Long Value
        /// </summary>
        [JsonPropertyName("availableLongVal")]
        public decimal? AvailableLongValue { get; set; }
        /// <summary>
        /// Available Short Value
        /// </summary>
        [JsonPropertyName("availableShortVal")]
        public decimal? AvailableShortValue { get; set; }
        /// <summary>
        /// Maximum Position Long Value
        /// </summary>
        [JsonPropertyName("maxPositionLongVal")]
        public decimal? MaxPositionLongValue { get; set; }
        /// <summary>
        /// Maximum Position Short Value
        /// </summary>
        [JsonPropertyName("maxPositionShortVal")]
        public decimal? MaxPositionShortValue { get; set; }
    }
}
