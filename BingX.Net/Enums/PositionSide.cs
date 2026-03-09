using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Position side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionSide>))]
    public enum PositionSide
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
        Short,
        /// <summary>
        /// ["<c>BOTH</c>"] Both
        /// </summary>
        [Map("BOTH")]
        Both
    }
}
