using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Cancel result
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXCancelsResult
    {
        /// <summary>
        /// Successfully canceled orders
        /// </summary>
        [JsonPropertyName("orders")]
        public BingXOrder[] Orders { get; set; } = Array.Empty<BingXOrder>();

        /// <summary>
        /// Failed to cancel orders
        /// </summary>
        [JsonPropertyName("fails")]
        public BingXOrderError[] Fails { get; set; } = [];
    }
}
