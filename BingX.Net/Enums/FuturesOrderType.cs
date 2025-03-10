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
        /// Market order
        /// </summary>
        [Map("MARKET")]
        Market,
        /// <summary>
        /// Limit order
        /// </summary>
        [Map("LIMIT")]
        Limit,
        /// <summary>
        /// Take profit market order
        /// </summary>
        [Map("TAKE_PROFIT_MARKET")]
        TakeProfitMarket,
        /// <summary>
        /// Stop market order
        /// </summary>
        [Map("STOP_MARKET")]
        StopMarket,
        /// <summary>
        /// Stop limit order
        /// </summary>
        [Map("STOP")]
        StopLimit,
        /// <summary>
        /// Take profit limit order
        /// </summary>
        [Map("TAKE_PROFIT")]
        TakeProfitLimit,
        /// <summary>
        /// Stop Limit Order with Trigger
        /// </summary>
        [Map("TRIGGER_LIMIT")]
        TriggerLimit,
        /// <summary>
        /// Stop Market Order with Trigger
        /// </summary>
        [Map("TRIGGER_MARKET")]
        TriggerMarket,
        /// <summary>
        /// Trailing stop market order
        /// </summary>
        [Map("TRAILING_STOP_MARKET")]
        TrailingStopMarket,
        /// <summary>
        /// Trailing TakeProfit or StopLoss
        /// </summary>
        [Map("TRAILING_TP_SL")]
        TrailingTpSl
    }
}
