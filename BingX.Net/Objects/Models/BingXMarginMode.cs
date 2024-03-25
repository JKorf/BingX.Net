using BingX.Net.Enums;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Margin mode info
    /// </summary>
    public record BingXMarginMode
    {
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("marginType")]
        public MarginMode MarginMode { get; set; }
    }
}
