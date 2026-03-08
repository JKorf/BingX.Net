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
        /// ["<c>longLeverage</c>"] Long position leverage
        /// </summary>
        [JsonPropertyName("longLeverage")]
        public decimal LongLeverage { get; set; }
        /// <summary>
        /// ["<c>shortLeverage</c>"] Short position leverage
        /// </summary>
        [JsonPropertyName("shortLeverage")]
        public decimal ShortLeverage { get; set; }
        /// <summary>
        /// ["<c>maxLongLeverage</c>"] Max long position leverage
        /// </summary>
        [JsonPropertyName("maxLongLeverage")]
        public decimal MaxLongLeverage { get; set; }
        /// <summary>
        /// ["<c>maxShortLeverage</c>"] Max short position leverage
        /// </summary>
        [JsonPropertyName("maxShortLeverage")]
        public decimal MaxShortLeverage { get; set; }
        /// <summary>
        /// ["<c>availableLongVol</c>"] Available Long Volume
        /// </summary>
        [JsonPropertyName("availableLongVol")]
        public decimal? AvailableLongVolume { get; set; }
        /// <summary>
        /// ["<c>availableShortVol</c>"] Available Short Volume
        /// </summary>
        [JsonPropertyName("availableShortVol")]
        public decimal? AvailableShortVolume { get; set; }
        /// <summary>
        /// ["<c>availableLongVal</c>"] Available Long Value
        /// </summary>
        [JsonPropertyName("availableLongVal")]
        public decimal? AvailableLongValue { get; set; }
        /// <summary>
        /// ["<c>availableShortVal</c>"] Available Short Value
        /// </summary>
        [JsonPropertyName("availableShortVal")]
        public decimal? AvailableShortValue { get; set; }
        /// <summary>
        /// ["<c>maxPositionLongVal</c>"] Maximum Position Long Value
        /// </summary>
        [JsonPropertyName("maxPositionLongVal")]
        public decimal? MaxPositionLongValue { get; set; }
        /// <summary>
        /// ["<c>maxPositionShortVal</c>"] Maximum Position Short Value
        /// </summary>
        [JsonPropertyName("maxPositionShortVal")]
        public decimal? MaxPositionShortValue { get; set; }
    }
}
