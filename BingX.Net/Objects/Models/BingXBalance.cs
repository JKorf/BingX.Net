using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXBalanceWrapper
    {
        [JsonPropertyName("balances")]
        public BingXBalance[] Balances { get; set; } = Array.Empty<BingXBalance>();
    }

    /// <summary>
    /// Balance info
    /// </summary>
    public record BingXBalance
    {
        /// <summary>
        /// Asset name
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Free
        /// </summary>
        [JsonPropertyName("free")]
        public decimal Free { get; set; }
        /// <summary>
        /// Locked
        /// </summary>
        [JsonPropertyName("locked")]
        public decimal Locked { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public decimal Total => Free + Locked;
    }
}
