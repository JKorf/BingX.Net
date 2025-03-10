using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Margin asset
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXMarginAsset
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Total quantity
        /// </summary>
        [JsonPropertyName("totalAmount")]
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// Available transfer
        /// </summary>
        [JsonPropertyName("availableTransfer")]
        public decimal AvailableTransfer { get; set; }
        /// <summary>
        /// Latest mortgage quantity
        /// </summary>
        [JsonPropertyName("latestMortgageAmount")]
        public decimal LatestMortgageQuantity { get; set; }
    }


}
