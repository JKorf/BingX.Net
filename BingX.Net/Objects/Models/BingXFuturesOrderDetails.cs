using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BingX.Net.Enums;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesOrderDetailsWrapper
    {
        [JsonPropertyName("order")]
        public BingXFuturesOrderDetails Order { get; set; } = null!;
    }

    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesOrdersDetailsWrapper
    {
        [JsonPropertyName("orders")]
        public BingXFuturesOrderDetails[] Orders { get; set; } = null!;
    }

    /// <summary>
    /// Order info
    /// </summary>
    public record BingXFuturesOrderDetails
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
        /// Order price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("origQty")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal? QuantityFilled { get; set; }
        /// <summary>
        /// Value filled (price * quantity)
        /// </summary>
        [JsonPropertyName("cumQuote")]
        public decimal? ValueFilled { get; set; }
        /// <summary>
        /// Order average fill price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// Profit
        /// </summary>
        [JsonPropertyName("profit")]
        public decimal? Profit { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal? Fee { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public string Leverage { get; set; } = string.Empty;
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
        [JsonPropertyName("clientOrderId")]
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
        [JsonInclude, JsonPropertyName("actPrice")]
        internal decimal? TriggerPriceInt { get => TriggerPrice; set => TriggerPrice = value; }
        /// <summary>
        /// Trailing stop rate
        /// </summary>
        [JsonPropertyName("trailingStopRate")]
        public decimal? TrailingStopRate { get; set; }
        /// <summary>
        /// Trailing stop distance
        /// </summary>
        [JsonPropertyName("trailingStopDistance")]
        public decimal? TrailingStopDistance { get; set; }
        /// <summary>
        /// Close position
        /// </summary>
        [JsonPropertyName("closePosition")]
        public bool? ClosePosition { get; set; }
        /// <summary>
        /// Stop loss order info
        /// </summary>
        [JsonPropertyName("stopLoss")]
        public BingXStopOrder? StopLoss { get; set; }
        /// <summary>
        /// Take profit order info
        /// </summary>
        [JsonPropertyName("takeProfit")]
        public BingXStopOrder? TakeProfit { get; set; }
        /// <summary>
        /// Trigger order id
        /// </summary>
        [JsonPropertyName("triggerOrderId")]
        public long? TriggerOrderId { get; set; }
        /// <summary>
        /// Is post only order
        /// </summary>
        [JsonPropertyName("postOnly")]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// Is part of a Time Weight Average Price Order
        /// </summary>
        [JsonPropertyName("isTwap")]
        public bool IsTwap { get; set; }
        /// <summary>
        /// The main order id if this order is part of a Twap order
        /// </summary>
        [JsonPropertyName("mainOrderId")]
        public string? MainOrderId { get; set; }
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public long? PositionId { get; set; }
    }

    /// <summary>
    /// Stop order info
    /// </summary>
    [SerializationModel]
    public record BingXStopOrder
    {
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public TakeProfitStopLossMode? Type { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Trigger type
        /// </summary>
        [JsonPropertyName("workingType")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// Stop guarenteed
        /// </summary>
        [JsonPropertyName("stopGuaranteed")]
        public bool? StopGuaranteed { get; set; }
    }
}
