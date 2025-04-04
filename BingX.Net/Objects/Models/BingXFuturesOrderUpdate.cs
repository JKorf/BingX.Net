using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Enums;

namespace BingX.Net.Objects.Models
{
    [SerializationModel]
    internal record BingXFuturesOrderUpdateWrapper : BingXSocketUpdate
    {
        [JsonPropertyName("o")]
        public BingXFuturesOrderUpdate Data { get; set; } = null!;
    }

    /// <summary>
    /// Order update
    /// </summary>
    [SerializationModel]
    public record BingXFuturesOrderUpdate
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
        public FuturesOrderType Type { get; set; }
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
        /// Quantity filled in quote asset
        /// </summary>
        [JsonPropertyName("Z")]
        public decimal? VolumeFilled { get; set; }

        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("sp")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger type
        /// </summary>
        [JsonPropertyName("wt")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// Average order price
        /// </summary>
        [JsonPropertyName("ap")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("ps")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Profit and loss of the transaction
        /// </summary>
        [JsonPropertyName("rp")]
        public decimal? ProfitAndLoss { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("ro")]
        public bool? ReduceOnly { get; set; }
    }
}
