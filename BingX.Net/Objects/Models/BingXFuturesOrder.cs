using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// ["<c>quantity</c>"] Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Order price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
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
        /// ["<c>clientOrderID</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID")]
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
        /// <summary>
        /// ["<c>closePosition</c>"] Close position
        /// </summary>
        [JsonPropertyName("closePosition")]
        public bool? ClosePosition { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>executedQty</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("executedQty")]
        public decimal? QuantityFilled { get; set; }
    }
}
