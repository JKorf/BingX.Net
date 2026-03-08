using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXOrderDetailsWrapper
    {
        [JsonPropertyName("orders")]
        public BingXOrderDetails[] Orders { get; set; } = Array.Empty<BingXOrderDetails>();
    }

    /// <summary>
    /// Order info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXOrderDetails
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Order price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>StopPrice</c>"] Stop price
        /// </summary>
        [JsonPropertyName("StopPrice")] // Actually is with uppercase
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// ["<c>origQty</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("origQty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>executedQty</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>cummulativeQuoteQty</c>"] Total value filled (price * quantity)
        /// </summary>
        [JsonPropertyName("cummulativeQuoteQty")]
        public decimal ValueFilled { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Order type
        /// </summary>
        [JsonPropertyName("type")]
        public OrderType Type { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>clientOrderID</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Creation timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update timestamp
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>origQuoteOrderQty</c>"] Original quote order quantity
        /// </summary>
        [JsonPropertyName("origQuoteOrderQty")]
        public decimal? QuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee paid
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>feeAsset</c>"] The fee asset
        /// </summary>
        [JsonPropertyName("feeAsset")]
        public string? FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>avgPrice</c>"] Average fill price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal? AveragePrice { get; set; }
    }
}
