using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>type</c>"] Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Order price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>origQty</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("origQty")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// ["<c>executedQty</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal? QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>cumQuote</c>"] Value filled (price * quantity)
        /// </summary>
        [JsonPropertyName("cumQuote")]
        public decimal? ValueFilled { get; set; }
        /// <summary>
        /// ["<c>avgPrice</c>"] Order average fill price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// ["<c>profit</c>"] Profit
        /// </summary>
        [JsonPropertyName("profit")]
        public decimal? Profit { get; set; }
        /// <summary>
        /// ["<c>commission</c>"] Fee
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal? Fee { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Create time
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Last update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public string Leverage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>stopPrice</c>"] Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// ["<c>workingType</c>"] Price trigger type
        /// </summary>
        [JsonPropertyName("workingType")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>stopGuaranteed</c>"] Stop guaranteed
        /// </summary>
        [JsonPropertyName("stopGuaranteed")]
        public bool? StopGuaranteed { get; set; }
        /// <summary>
        /// ["<c>timeInForce</c>"] Time in force
        /// </summary>
        [JsonPropertyName("timeInForce")]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// ["<c>priceRate</c>"] Trailing stop price rate
        /// </summary>
        [JsonPropertyName("priceRate")]
        public decimal? PriceRate { get; set; }
        /// <summary>
        /// ["<c>reduceOnly</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>activationPrice</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("activationPrice")]
        public decimal? TriggerPrice { get; set; }
        [JsonInclude, JsonPropertyName("actPrice")]
        internal decimal? TriggerPriceInt { get => TriggerPrice; set => TriggerPrice = value; }
        /// <summary>
        /// ["<c>trailingStopRate</c>"] Trailing stop rate
        /// </summary>
        [JsonPropertyName("trailingStopRate")]
        public decimal? TrailingStopRate { get; set; }
        /// <summary>
        /// ["<c>trailingStopDistance</c>"] Trailing stop distance
        /// </summary>
        [JsonPropertyName("trailingStopDistance")]
        public decimal? TrailingStopDistance { get; set; }
        /// <summary>
        /// ["<c>closePosition</c>"] Close position
        /// </summary>
        [JsonPropertyName("closePosition")]
        public bool? ClosePosition { get; set; }
        /// <summary>
        /// ["<c>stopLoss</c>"] Stop loss order info
        /// </summary>
        [JsonPropertyName("stopLoss")]
        public BingXStopOrder? StopLoss { get; set; }
        /// <summary>
        /// ["<c>takeProfit</c>"] Take profit order info
        /// </summary>
        [JsonPropertyName("takeProfit")]
        public BingXStopOrder? TakeProfit { get; set; }
        /// <summary>
        /// ["<c>triggerOrderId</c>"] Trigger order id
        /// </summary>
        [JsonPropertyName("triggerOrderId")]
        public long? TriggerOrderId { get; set; }
        /// <summary>
        /// ["<c>postOnly</c>"] Is post only order
        /// </summary>
        [JsonPropertyName("postOnly")]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// ["<c>isTwap</c>"] Is part of a Time Weight Average Price Order
        /// </summary>
        [JsonPropertyName("isTwap")]
        public bool IsTwap { get; set; }
        /// <summary>
        /// ["<c>mainOrderId</c>"] The main order id if this order is part of a Twap order
        /// </summary>
        [JsonPropertyName("mainOrderId")]
        public string? MainOrderId { get; set; }
        /// <summary>
        /// ["<c>positionId</c>"] Position id
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
        /// ["<c>type</c>"] Order type
        /// </summary>
        [JsonPropertyName("type")]
        public TakeProfitStopLossMode? Type { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// ["<c>stopPrice</c>"] Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>workingType</c>"] Trigger type
        /// </summary>
        [JsonPropertyName("workingType")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// ["<c>stopGuaranteed</c>"] Stop guarenteed
        /// </summary>
        [JsonPropertyName("stopGuaranteed")]
        public bool? StopGuaranteed { get; set; }
    }
}
