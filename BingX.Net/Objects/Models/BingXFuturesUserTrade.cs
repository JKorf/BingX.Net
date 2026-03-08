using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
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
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientOrderID</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID")]
        public string ClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Quantity
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Value traded
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Value { get; set; }
        /// <summary>
        /// ["<c>commission</c>"] Fee paid
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>filledTm</c>"] Trade time
        /// </summary>
        [JsonPropertyName("filledTm")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>liquidatedPrice</c>"] Liquidation price
        /// </summary>
        [JsonPropertyName("liquidatedPrice")]
        public decimal? LiquidatedPrice { get; set; }
        /// <summary>
        /// ["<c>liquidatedMarginRatio</c>"] Liquidation margin ratio
        /// </summary>
        [JsonPropertyName("liquidatedMarginRatio")]
        public decimal? LiquidatedMarginRatio { get; set; }
        /// <summary>
        /// ["<c>workingType</c>"] Stop price trigger type
        /// </summary>
        [JsonPropertyName("workingType")]
        public TriggerType? TriggerPriceType { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Trade side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide? PositionSide { get; set; }
    }
}
