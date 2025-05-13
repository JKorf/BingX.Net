using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Multi asset mode rules
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXMultiAssetRules
    {
        /// <summary>
        /// Margin assets
        /// </summary>
        [JsonPropertyName("marginAssets")]
        public string MarginAssets { get; set; } = string.Empty;
        /// <summary>
        /// Loan to value ratio
        /// </summary>
        [JsonPropertyName("ltv")]
        public string Ltv { get; set; } = string.Empty;
        /// <summary>
        /// Collateral value ratio
        /// </summary>
        [JsonPropertyName("collateralValueRatio")]
        public string CollateralValueRatio { get; set; } = string.Empty;
        /// <summary>
        /// Max transfer
        /// </summary>
        [JsonPropertyName("maxTransfer")]
        public decimal? MaxTransfer { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
    }


}
