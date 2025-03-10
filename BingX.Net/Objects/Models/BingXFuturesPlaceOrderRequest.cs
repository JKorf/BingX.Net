using BingX.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Futures place order request for bulk placement
    /// </summary>
    [SerializationModel]
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
        [JsonPropertyName("reduceOnly"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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
        public TimeInForce? TimeInForce { get; set; }
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

        /// <summary>
        /// Stop loss order parameters
        /// </summary>
        [JsonIgnore]
        public BingXStopOrderRequest? StopLoss { get; set; }
        /// <summary>
        /// Take profit order parameters
        /// </summary>
        [JsonIgnore]
        public BingXStopOrderRequest? TakeProfit { get; set; }

        /// <summary>
        /// Internal serialization parameter, use StopLoss for setting stop loss parameters
        /// </summary>
        [JsonPropertyName("stopLoss"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StopLossStr { get; internal set; }
        /// <summary>
        /// Internal serialization parameter, use TakeProfit for setting take profit parameters
        /// </summary>
        [JsonPropertyName("takeProfit"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TakeProfitStr { get; internal set; }
    }

    /// <summary>
    /// Stop order info
    /// </summary>
    public record BingXStopOrderRequest
    {
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public TakeProfitStopLossMode Type { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal StopPrice { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        /// <summary>
        /// Trigger type
        /// </summary>
        [JsonPropertyName("workingType"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// Stop guarantied
        /// </summary>
        [JsonPropertyName("stopGuaranteed"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? StopGuaranteed { get; set; }
    }

}
