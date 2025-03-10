using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Leverage info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXLeverage
    {
        /// <summary>
        /// Long position leverage
        /// </summary>
        [JsonPropertyName("longLeverage")]
        public decimal LongLeverage { get; set; }
        /// <summary>
        /// Short position leverage
        /// </summary>
        [JsonPropertyName("shortLeverage")]
        public decimal ShortLeverage { get; set; }
        /// <summary>
        /// Max long position leverage
        /// </summary>
        [JsonPropertyName("maxLongLeverage")]
        public decimal MaxLongLeverage { get; set; }
        /// <summary>
        /// Max short position leverage
        /// </summary>
        [JsonPropertyName("maxShortLeverage")]
        public decimal MaxShortLeverage { get; set; }
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
