using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesUserTradeDetailsWrapper
    {
        [JsonPropertyName("fill_history_orders")]
        public BingXFuturesUserTradeDetails[] Trades { get; set; } = Array.Empty<BingXFuturesUserTradeDetails>();
        //[JsonInclude, JsonPropertyName("fill_history_orders")]
        //internal IEnumerable<BingXFuturesUserTradeDetails> TradeHistory { set => Trades = value; }
    }

    /// <summary>
    /// User trade info
    /// </summary>
    public record BingXFuturesUserTradeDetails
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
        /// ["<c>tradeId</c>"] Trade id
        /// </summary>
        [JsonPropertyName("tradeId")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>qty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>quoteQty</c>"] Value
        /// </summary>
        [JsonPropertyName("quoteQty")]
        public decimal Value { get; set; }
        /// <summary>
        /// ["<c>commission</c>"] Fee paid
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>commissionAsset</c>"] Fee asset
        /// </summary>
        [JsonPropertyName("commissionAsset")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>filledTime</c>"] Trade time
        /// </summary>
        [JsonPropertyName("filledTime")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Trade side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// ["<c>role</c>"] Trade role
        /// </summary>
        [JsonPropertyName("role")]
        public Role? Role { get; set; }
    }
}
