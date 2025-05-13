using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Trade side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TradeSide>))]
    public enum TradeSide
    {
        /// <summary>
        /// Long position
        /// </summary>
        [Map("LONG")]
        Long,
        /// <summary>
        /// Short position
        /// </summary>
        [Map("SHORT")]
        Short
    }
}
