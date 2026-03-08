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
        /// ["<c>e</c>"] Event name
        /// </summary>
        [JsonPropertyName("e")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>E</c>"] Event timestamp
        /// </summary>
        [JsonPropertyName("E")]
        public DateTime EventTime { get; set; }
    }
}
