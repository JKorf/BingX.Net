using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXFuturesTrade
    {
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("fillId")]
        public long TradeId { get; set; }
        /// <summary>
        /// Id for V1 API
        /// </summary>
        [JsonPropertyName("id")]
        public long IdV1 { set => TradeId = value; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Trade value in quote asset
        /// </summary>
        [JsonPropertyName("quoteQty")]
        public decimal Value { get; set; }
        /// <summary>
        /// timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Whether buyer was the maker
        /// </summary>
        [JsonPropertyName("isBuyerMaker")]
        public bool BuyerIsMaker { get; set; }
    }
}
