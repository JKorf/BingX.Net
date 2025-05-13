using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXUserTradeWrapper
    {
        [JsonPropertyName("fills")]
        public BingXUserTrade[] Trades { get; set; } = Array.Empty<BingXUserTrade>();
    }

    /// <summary>
    /// User trade info
    /// </summary>
    public record BingXUserTrade
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity traded
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Value traded in quote asset
        /// </summary>
        [JsonPropertyName("quoteQty")]
        public decimal Value { get; set; }
        /// <summary>
        /// Fee paid
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Fee asset
        /// </summary>
        [JsonPropertyName("commissionAsset")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// Trade time
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Is buyer
        /// </summary>
        [JsonPropertyName("isBuyer")]
        public bool IsBuyer { get; set; }
        /// <summary>
        /// Is maker
        /// </summary>
        [JsonPropertyName("isMaker")]
        public bool IsMaker { get; set; }
    }
}
