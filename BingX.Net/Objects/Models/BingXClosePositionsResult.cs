using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Close positions result
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXClosePositionsResult
    {
        /// <summary>
        /// Order ids of orders which successfully closed a position
        /// </summary>
        [JsonPropertyName("success")]
        public long[]? Success { get; set; }
        /// <summary>
        /// Order ids or orders which failed to close a position
        /// </summary>
        [JsonPropertyName("failed")]
        public long[]? Failed { get; set; }
    }
}
