using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trading rules
    /// </summary>
    public record BingXTradingRules
    {
        /// <summary>
        /// Min quantity for an order
        /// </summary>
        [JsonPropertyName("minSizeCoin")]
        public decimal? MinOrderQuantity { get; set; }
        /// <summary>
        /// Min order value in USD
        /// </summary>
        [JsonPropertyName("minSizeUsd")]
        public decimal? MinOrderValue { get; set; }
        /// <summary>
        /// Max number of open orders
        /// </summary>
        [JsonPropertyName("maxNumOrder")]
        public int? MaxOrders { get; set; }
        /// <summary>
        /// If spread protection is enabled and when a strategy order is triggered, if the spread between the latest price and mark price exceeds this threshold, the order will fail
        /// </summary>
        [JsonPropertyName("protectionThreshold")]
        public decimal? SpreadProtectThreshold { get; set; }
        /// <summary>
        /// Buy max price ratio
        /// </summary>
        [JsonPropertyName("buyMaxPrice")]
        public decimal? BuyMaxPriceRatio { get; set; }
        /// <summary>
        /// Buy min price ratio
        /// </summary>
        [JsonPropertyName("buyMinPrice")]
        public decimal? BuyMinPriceRatio { get; set; }
        /// <summary>
        /// Sell max price ratio
        /// </summary>
        [JsonPropertyName("sellMaxPrice")]
        public decimal? SellMaxPriceRatio { get; set; }
        /// <summary>
        /// Sell min price ratio
        /// </summary>
        [JsonPropertyName("sellMinPrice")]
        public decimal? SellMinPriceRatio { get; set; }
        /// <summary>
        /// Market order price tolerance ratio
        /// </summary>
        [JsonPropertyName("marketRatio")]
        public decimal? MarketRatio { get; set; }
    }
}
