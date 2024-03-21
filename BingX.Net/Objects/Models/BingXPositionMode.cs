using BingX.Net.Enums;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Position mode
    /// </summary>
    public record BingXPositionMode
    {
        /// <summary>
        /// Position mode
        /// </summary>
        [JsonPropertyName("dualSidePosition")]
        public PositionMode PositionMode { get; set; }
    }
}
