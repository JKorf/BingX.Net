using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Leverage info
    /// </summary>
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
    }
}
