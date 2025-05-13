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
    /// OCO order info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXOcoOrder
    {
        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonPropertyName("transactionTime")]
        public DateTime? TransactionTime { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string ClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("orderType")]
        public OcoOrderType? OrderType { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide? Side { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Order list id
        /// </summary>
        [JsonPropertyName("orderListId")]
        public string OrderListId { get; set; } = string.Empty;
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus? Status { get; set; }
    }
}
