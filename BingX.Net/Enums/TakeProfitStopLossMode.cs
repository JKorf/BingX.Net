using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Take Profit / Stop Loss mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TakeProfitStopLossMode>))]
    public enum TakeProfitStopLossMode
    {
        /// <summary>
        /// Stop loss market order
        /// </summary>
        [Map("STOP_MARKET")]
        StopMarket,
        /// <summary>
        /// Stop loss limit order
        /// </summary>
        [Map("STOP")]
        Stop,
        /// <summary>
        /// Take profit market order
        /// </summary>
        [Map("TAKE_PROFIT_MARKET")]
        TakeProfitMarket,
        /// <summary>
        /// Take profit limit order
        /// </summary>
        [Map("TAKE_PROFIT")]
        TakeProfit
    }
}
