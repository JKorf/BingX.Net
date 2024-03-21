using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Close positions result
    /// </summary>
    public record BingXClosePositionsResult
    {
        /// <summary>
        /// Order ids of orders which successfully closed a position
        /// </summary>
        [JsonPropertyName("success")]
        public IEnumerable<long>? Success { get; set; }
        /// <summary>
        /// Order ids or orders which failed to close a position
        /// </summary>
        [JsonPropertyName("failed")]
        public IEnumerable<long>? Failed { get; set; }
    }
}
