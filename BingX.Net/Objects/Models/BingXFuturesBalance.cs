using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Futures balances info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXFuturesBalance
    {
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>balance</c>"] Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal? Balance { get; set; }
        /// <summary>
        /// ["<c>equity</c>"] Net asset value
        /// </summary>
        [JsonPropertyName("equity")]
        public decimal? Equity { get; set; }
        /// <summary>
        /// ["<c>unrealizedProfit</c>"] Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("unrealizedProfit")]
        public decimal? UnrealizedProfit { get; set; }
        /// <summary>
        /// ["<c>realisedProfit</c>"] Realized profit
        /// </summary>
        [JsonPropertyName("realisedProfit")]
        public decimal RealizedProfit { get; set; }
        /// <summary>
        /// ["<c>availableMargin</c>"] Available margin
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public decimal? AvailableMargin { get; set; }
        /// <summary>
        /// ["<c>usedMargin</c>"] Used margin
        /// </summary>
        [JsonPropertyName("usedMargin")]
        public decimal? UsedMargin { get; set; }
        /// <summary>
        /// ["<c>freezedMargin</c>"] Frozen margin
        /// </summary>
        [JsonPropertyName("freezedMargin")]
        public decimal? FrozenMargin { get; set; }
        /// <summary>
        /// ["<c>shortUid</c>"] Short uid
        /// </summary>
        [JsonPropertyName("shortUid")]
        public string? ShortUid { get; set; }
    }
}
