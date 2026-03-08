using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>totalAmount</c>"] Total quantity
        /// </summary>
        [JsonPropertyName("totalAmount")]
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// ["<c>availableTransfer</c>"] Available transfer
        /// </summary>
        [JsonPropertyName("availableTransfer")]
        public decimal AvailableTransfer { get; set; }
        /// <summary>
        /// ["<c>latestMortgageAmount</c>"] Latest mortgage quantity
        /// </summary>
        [JsonPropertyName("latestMortgageAmount")]
        public decimal LatestMortgageQuantity { get; set; }
    }


}
