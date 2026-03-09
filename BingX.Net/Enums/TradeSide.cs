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
        /// ["<c>LONG</c>"] Long position
        /// </summary>
        [Map("LONG")]
        Long,
        /// <summary>
        /// ["<c>SHORT</c>"] Short position
        /// </summary>
        [Map("SHORT")]
        Short
    }
}
