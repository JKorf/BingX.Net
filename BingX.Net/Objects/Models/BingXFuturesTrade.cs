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
        /// ["<c>fillId</c>"] Trade id
        /// </summary>
        [JsonPropertyName("fillId")]
        public long TradeId { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id for V1 API
        /// </summary>
        [JsonPropertyName("id")]
        public long IdV1 { set => TradeId = value; }
        /// <summary>
        /// ["<c>price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>qty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>quoteQty</c>"] Trade value in quote asset
        /// </summary>
        [JsonPropertyName("quoteQty")]
        public decimal Value { get; set; }
        /// <summary>
        /// ["<c>time</c>"] timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        [JsonInclude, JsonPropertyName("ts")]
        internal DateTime TimestampInt { set => Timestamp = value; }
        /// <summary>
        /// ["<c>isBuyerMaker</c>"] Whether buyer was the maker
        /// </summary>
        [JsonPropertyName("isBuyerMaker")]
        public bool BuyerIsMaker { get; set; }
    }
}
