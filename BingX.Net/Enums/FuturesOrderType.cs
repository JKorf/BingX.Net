using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesOrderType>))]
    public enum FuturesOrderType
    {
        /// <summary>
        /// ["<c>MARKET</c>"] Market order
        /// </summary>
        [Map("MARKET")]
        Market,
        /// <summary>
        /// ["<c>LIMIT</c>"] Limit order
        /// </summary>
        [Map("LIMIT")]
        Limit,
        /// <summary>
        /// ["<c>TAKE_PROFIT_MARKET</c>"] Take profit market order
        /// </summary>
        [Map("TAKE_PROFIT_MARKET")]
        TakeProfitMarket,
        /// <summary>
        /// ["<c>STOP_MARKET</c>"] Stop market order
        /// </summary>
        [Map("STOP_MARKET")]
        StopMarket,
        /// <summary>
        /// ["<c>STOP</c>"] Stop limit order
        /// </summary>
        [Map("STOP")]
        StopLimit,
        /// <summary>
        /// ["<c>TAKE_PROFIT</c>"] Take profit limit order
        /// </summary>
        [Map("TAKE_PROFIT")]
        TakeProfitLimit,
        /// <summary>
        /// ["<c>TRIGGER_LIMIT</c>"] Stop Limit Order with Trigger
        /// </summary>
        [Map("TRIGGER_LIMIT")]
        TriggerLimit,
        /// <summary>
        /// ["<c>TRIGGER_MARKET</c>"] Stop Market Order with Trigger
        /// </summary>
        [Map("TRIGGER_MARKET")]
        TriggerMarket,
        /// <summary>
        /// ["<c>TRAILING_STOP_MARKET</c>"] Trailing stop market order
        /// </summary>
        [Map("TRAILING_STOP_MARKET")]
        TrailingStopMarket,
        /// <summary>
        /// ["<c>TRAILING_TP_SL</c>"] Trailing TakeProfit or StopLoss
        /// </summary>
        [Map("TRAILING_TP_SL")]
        TrailingTpSl
    }
}
