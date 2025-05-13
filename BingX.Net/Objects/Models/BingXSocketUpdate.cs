using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Socket update
    /// </summary>
    [SerializationModel]
    public record BingXSocketUpdate
    {
        /// <summary>
        /// Event name
        /// </summary>
        [JsonPropertyName("e")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// Event timestamp
        /// </summary>
        [JsonPropertyName("E")]
        public DateTime EventTime { get; set; }
    }
}
