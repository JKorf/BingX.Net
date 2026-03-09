using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderType>))]
    public enum OrderType
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
        /// ["<c>TAKE_STOP_LIMIT</c>"] Stop limit order
        /// </summary>
        [Map("TAKE_STOP_LIMIT")]
        StopLimit,
        /// <summary>
        /// ["<c>TAKE_STOP_MARKET</c>"] Stop market order
        /// </summary>
        [Map("TAKE_STOP_MARKET")]
        StopMarket,
        /// <summary>
        /// ["<c>TRIGGER_LIMIT</c>"] Trigger limit order
        /// </summary>
        [Map("TRIGGER_LIMIT")]
        TriggerLimit,
        /// <summary>
        /// ["<c>TRIGGER_MARKET</c>"] Trigger market order
        /// </summary>
        [Map("TRIGGER_MARKET")]
        TriggerMarket
    }
}
