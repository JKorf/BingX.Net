using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Enums;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Order update
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXOrderUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("C")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("S")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("o")]
        public OrderType Type { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("q")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("x")]
        public string EventType { get; set; } = string.Empty;
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("X")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("i")]
        public long OrderId { get; set; }
        /// <summary>
        /// Quantity of the last fill for this order
        /// </summary>
        [JsonPropertyName("l")]
        public decimal? LastFillQuantity { get; set; }
        /// <summary>
        /// Price of the last fill for this order
        /// </summary>
        [JsonPropertyName("L")]
        public decimal? LastFillPrice { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("z")]
        public decimal? QuantityFilled { get; set; }
        /// <summary>
        /// Fee paid
        /// </summary>
        [JsonPropertyName("n")]
        public decimal? Fee { get; set; }
        /// <summary>
        /// Fee asset
        /// </summary>
        [JsonPropertyName("N")]
        public string? FeeAsset { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Creation time
        /// </summary>
        [JsonPropertyName("O")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("t")]
        public long TradeId { get; set; }
        /// <summary>
        /// Quantity filled in quote asset
        /// </summary>
        [JsonPropertyName("Z")]
        public decimal? VolumeFilled { get; set; }
        /// <summary>
        /// Last fill value
        /// </summary>
        [JsonPropertyName("Y")]
        public decimal? LastFillValue { get; set; }
        /// <summary>
        /// Original quote order quantity
        /// </summary>
        [JsonPropertyName("Q")]
        public decimal? QuoteOrderQuantity { get; set; }
    }
}
