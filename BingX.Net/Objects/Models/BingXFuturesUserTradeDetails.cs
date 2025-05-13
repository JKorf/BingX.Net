using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Collections.Generic;
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
        /// Trade id
        /// </summary>
        [JsonPropertyName("tradeId")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        [JsonPropertyName("quoteQty")]
        public decimal Value { get; set; }
        /// <summary>
        /// Fee paid
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Fee asset
        /// </summary>
        [JsonPropertyName("commissionAsset")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// Trade time
        /// </summary>
        [JsonPropertyName("filledTime")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Trade side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// Trade role
        /// </summary>
        [JsonPropertyName("role")]
        public Role? Role { get; set; }
    }
}
