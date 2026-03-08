using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// ["<c>marginAssets</c>"] Margin assets
        /// </summary>
        [JsonPropertyName("marginAssets")]
        public string MarginAssets { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ltv</c>"] Loan to value ratio
        /// </summary>
        [JsonPropertyName("ltv")]
        public string Ltv { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>collateralValueRatio</c>"] Collateral value ratio
        /// </summary>
        [JsonPropertyName("collateralValueRatio")]
        public string CollateralValueRatio { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>maxTransfer</c>"] Max transfer
        /// </summary>
        [JsonPropertyName("maxTransfer")]
        public decimal? MaxTransfer { get; set; }
        /// <summary>
        /// ["<c>indexPrice</c>"] Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
    }


}
