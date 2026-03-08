using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
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
        /// ["<c>list</c>"] List
        /// </summary>
        [JsonPropertyName("list")]
        public BingXTwapOrder[] List { get; set; } = Array.Empty<BingXTwapOrder>();
        /// <summary>
        /// ["<c>total</c>"] Total orders
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
        /// ["<c>mainOrderId</c>"] Main order id
        /// </summary>
        [JsonPropertyName("mainOrderId")]
        public long MainOrderId { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>priceType</c>"] Price type
        /// </summary>
        [JsonPropertyName("priceType")]
        public PriceType PriceType { get; set; }
        /// <summary>
        /// ["<c>priceVariance</c>"] Price variance
        /// </summary>
        [JsonPropertyName("priceVariance")]
        public decimal PriceVariance { get; set; }
        /// <summary>
        /// ["<c>triggerPrice</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>interval</c>"] Interval
        /// </summary>
        [JsonPropertyName("interval")]
        public int Interval { get; set; }
        /// <summary>
        /// ["<c>amountPerOrder</c>"] Quantity per order
        /// </summary>
        [JsonPropertyName("amountPerOrder")]
        public decimal QuantityPerOrder { get; set; }
        /// <summary>
        /// ["<c>totalAmount</c>"] Total quantity
        /// </summary>
        [JsonPropertyName("totalAmount")]
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// ["<c>orderStatus</c>"] Order status
        /// </summary>
        [JsonPropertyName("orderStatus")]
        public TwapOrderStatus OrderStatus { get; set; }
        /// <summary>
        /// ["<c>executedQty</c>"] Executed quantity
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal ExecutedQuantity { get; set; }
        /// <summary>
        /// ["<c>duration</c>"] Execution time, in seconds
        /// </summary>
        [JsonPropertyName("duration")]
        public decimal Duration { get; set; }
        /// <summary>
        /// ["<c>maxDuration</c>"] Maximum execution time execution time in seconds
        /// </summary>
        [JsonPropertyName("maxDuration")]
        public decimal MaxDuration { get; set; }
        /// <summary>
        /// ["<c>createdTime</c>"] Creation time
        /// </summary>
        [JsonPropertyName("createdTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
    }


}
