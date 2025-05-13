using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Collections.Generic;
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
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("StopPrice")] // Actually is with uppercase
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("origQty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Total value filled (price * quantity)
        /// </summary>
        [JsonPropertyName("cummulativeQuoteQty")]
        public decimal ValueFilled { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public OrderType Type { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Creation timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Update timestamp
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Original quote order quantity
        /// </summary>
        [JsonPropertyName("origQuoteOrderQty")]
        public decimal? QuoteQuantity { get; set; }
        /// <summary>
        /// Fee paid
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// The fee asset
        /// </summary>
        [JsonPropertyName("feeAsset")]
        public string? FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// Average fill price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal? AveragePrice { get; set; }
    }
}
