using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Cancel after timeout setting result
    /// </summary>
    public record BingXCancelAfterResult
    {
        /// <summary>
        /// Trigger time
        /// </summary>
        [JsonPropertyName("triggerTime")]
        public DateTime? TriggerTime { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
    }
}
