using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Twap order list
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXTwapOrders
    {
        /// <summary>
        /// List
        /// </summary>
        [JsonPropertyName("list")]
        public BingXTwapOrder[] List { get; set; } = Array.Empty<BingXTwapOrder>();
        /// <summary>
        /// Total orders
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Twap order info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXTwapOrder
    {
        /// <summary>
        /// Main order id
        /// </summary>
        [JsonPropertyName("mainOrderId")]
        public long MainOrderId { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Price type
        /// </summary>
        [JsonPropertyName("priceType")]
        public PriceType PriceType { get; set; }
        /// <summary>
        /// Price variance
        /// </summary>
        [JsonPropertyName("priceVariance")]
        public decimal PriceVariance { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// Interval
        /// </summary>
        [JsonPropertyName("interval")]
        public int Interval { get; set; }
        /// <summary>
        /// Quantity per order
        /// </summary>
        [JsonPropertyName("amountPerOrder")]
        public decimal QuantityPerOrder { get; set; }
        /// <summary>
        /// Total quantity
        /// </summary>
        [JsonPropertyName("totalAmount")]
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("orderStatus")]
        public TwapOrderStatus OrderStatus { get; set; }
        /// <summary>
        /// Executed quantity
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal ExecutedQuantity { get; set; }
        /// <summary>
        /// Execution time, in seconds
        /// </summary>
        [JsonPropertyName("duration")]
        public decimal Duration { get; set; }
        /// <summary>
        /// Maximum execution time execution time in seconds
        /// </summary>
        [JsonPropertyName("maxDuration")]
        public decimal MaxDuration { get; set; }
        /// <summary>
        /// Creation time
        /// </summary>
        [JsonPropertyName("createdTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
    }


}
