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
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Net asset value
        /// </summary>
        [JsonPropertyName("equity")]
        public decimal Equity { get; set; }
        /// <summary>
        /// Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("unrealizedProfit")]
        public decimal UnrealizedProfit { get; set; }
        /// <summary>
        /// Realized profit
        /// </summary>
        [JsonPropertyName("realisedProfit")]
        public decimal RealizedProfit { get; set; }
        /// <summary>
        /// Available margin
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public decimal AvailableMargin { get; set; }
        /// <summary>
        /// Used margin
        /// </summary>
        [JsonPropertyName("usedMargin")]
        public decimal UsedMargin { get; set; }
        /// <summary>
        /// Frozen margin
        /// </summary>
        [JsonPropertyName("freezedMargin")]
        public decimal FrozenMargin { get; set; }
        /// <summary>
        /// Short uid
        /// </summary>
        [JsonPropertyName("shortUid")]
        public string? ShortUid { get; set; }
    }
}
