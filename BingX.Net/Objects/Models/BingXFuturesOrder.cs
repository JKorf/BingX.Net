using CryptoExchange.Net.Converters.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BingX.Net.Enums;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesOrderWrapper
    {
        [JsonPropertyName("order")]
        public BingXFuturesOrder Order { get; set; } = null!;
    }

    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesOrdersWrapper
    {
        [JsonPropertyName("orders")]
        public BingXFuturesOrder[] Orders { get; set; } = null!;
    }

    /// <summary>
    /// Order info
    /// </summary>
    public record BingXFuturesOrder
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
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Price trigger type
        /// </summary>
        [JsonPropertyName("workingType")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Stop guaranteed
        /// </summary>
        [JsonPropertyName("stopGuaranteed")]
        public bool? StopGuaranteed { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonPropertyName("timeInForce")]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// Trailing stop price rate
        /// </summary>
        [JsonPropertyName("priceRate")]
        public decimal? PriceRate { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("activationPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Close position
        /// </summary>
        [JsonPropertyName("closePosition")]
        public bool? ClosePosition { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal? QuantityFilled { get; set; }
    }
}
