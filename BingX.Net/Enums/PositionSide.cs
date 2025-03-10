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
        /// Long position
        /// </summary>
        [Map("LONG")]
        Long,
        /// <summary>
        /// Short position
        /// </summary>
        [Map("SHORT")]
        Short,
        /// <summary>
        /// Both
        /// </summary>
        [Map("BOTH")]
        Both
    }
}
