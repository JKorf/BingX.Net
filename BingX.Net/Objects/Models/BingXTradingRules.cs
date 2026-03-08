using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trading rules
    /// </summary>
    public record BingXTradingRules
    {
        /// <summary>
        /// ["<c>minSizeCoin</c>"] Min quantity for an order
        /// </summary>
        [JsonPropertyName("minSizeCoin")]
        public decimal? MinOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>minSizeUsd</c>"] Min order value in USD
        /// </summary>
        [JsonPropertyName("minSizeUsd")]
        public decimal? MinOrderValue { get; set; }
        /// <summary>
        /// ["<c>maxNumOrder</c>"] Max number of open orders
        /// </summary>
        [JsonPropertyName("maxNumOrder")]
        public int? MaxOrders { get; set; }
        /// <summary>
        /// ["<c>protectionThreshold</c>"] If spread protection is enabled and when a strategy order is triggered, if the spread between the latest price and mark price exceeds this threshold, the order will fail
        /// </summary>
        [JsonPropertyName("protectionThreshold")]
        public decimal? SpreadProtectThreshold { get; set; }
        /// <summary>
        /// ["<c>buyMaxPrice</c>"] Buy max price ratio
        /// </summary>
        [JsonPropertyName("buyMaxPrice")]
        public decimal? BuyMaxPriceRatio { get; set; }
        /// <summary>
        /// ["<c>buyMinPrice</c>"] Buy min price ratio
        /// </summary>
        [JsonPropertyName("buyMinPrice")]
        public decimal? BuyMinPriceRatio { get; set; }
        /// <summary>
        /// ["<c>sellMaxPrice</c>"] Sell max price ratio
        /// </summary>
        [JsonPropertyName("sellMaxPrice")]
        public decimal? SellMaxPriceRatio { get; set; }
        /// <summary>
        /// ["<c>sellMinPrice</c>"] Sell min price ratio
        /// </summary>
        [JsonPropertyName("sellMinPrice")]
        public decimal? SellMinPriceRatio { get; set; }
        /// <summary>
        /// ["<c>marketRatio</c>"] Market order price tolerance ratio
        /// </summary>
        [JsonPropertyName("marketRatio")]
        public decimal? MarketRatio { get; set; }
    }
}
