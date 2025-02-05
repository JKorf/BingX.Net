using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Cancel result
    /// </summary>
    public record BingXCancelsResult
    {
        /// <summary>
        /// Successfully canceled orders
        /// </summary>
        [JsonPropertyName("orders")]
        public IEnumerable<BingXOrder> Orders { get; set; } = Array.Empty<BingXOrder>();

        /// <summary>
        /// Failed to cancel orders
        /// </summary>
        [JsonPropertyName("fails")]
        public IEnumerable<BingXOrderError> Fails { get; set; } = [];
    }
}
