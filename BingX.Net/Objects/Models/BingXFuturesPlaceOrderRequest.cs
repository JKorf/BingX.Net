using BingX.Net.Enums;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Futures place order request for bulk placement
    /// </summary>
    public record BingXFuturesPlaceOrderRequest
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType OrderType { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool? ReduceOnly { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPrice"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Price rate
        /// </summary>
        [JsonPropertyName("priceRate"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PriceRate { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Trigger type
        /// </summary>
        [JsonPropertyName("workingType"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonPropertyName("timeInForce"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TriggerType? TimeInForce { get; set; }
        /// <summary>
        /// Close position
        /// </summary>
        [JsonPropertyName("closePosition"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ClosePosition { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("activationPrice"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Stop guaranteed
        /// </summary>
        [JsonPropertyName("stopGuaranteed"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? StopGuaranteed { get; set; }


        // TODO How to pass the takeProfit/stopLoss fields..?
        //[JsonPropertyName("takeProfit"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public decimal? TakeProfit { get; set; }

        //[JsonPropertyName("takeProfit"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public FuturesOrderType? TakeProfit { get; set; }
        //[JsonPropertyName("stopLoss"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public FuturesOrderType? StopLoss { get; set; }

        ///// <summary>
        ///// Stop loss order
        ///// </summary>
        //[JsonPropertyName("stopLoss"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public BingXStopOrder? StopLoss { get; set; }
        ///// <summary>
        ///// Take profit order
        ///// </summary>
        //[JsonPropertyName("takeProfit"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public BingXStopOrder? TakeProfit { get; set; }
    }
}
