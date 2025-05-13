using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesUserTradeWrapper
    {
        [JsonPropertyName("fill_orders")]
        public BingXFuturesUserTrade[] Trades { get; set; } = Array.Empty<BingXFuturesUserTrade>();
    }

    /// <summary>
    /// User trade info
    /// </summary>
    public record BingXFuturesUserTrade
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID")]
        public string ClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Value traded
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Value { get; set; }
        /// <summary>
        /// Fee paid
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Trade time
        /// </summary>
        [JsonPropertyName("filledTm")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Liquidation price
        /// </summary>
        [JsonPropertyName("liquidatedPrice")]
        public decimal? LiquidatedPrice { get; set; }
        /// <summary>
        /// Liquidation margin ratio
        /// </summary>
        [JsonPropertyName("liquidatedMarginRatio")]
        public decimal? LiquidatedMarginRatio { get; set; }
        /// <summary>
        /// Stop price trigger type
        /// </summary>
        [JsonPropertyName("workingType")]
        public TriggerType? TriggerPriceType { get; set; }
        /// <summary>
        /// Trade side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide? PositionSide { get; set; }
    }
}
