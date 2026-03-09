using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Position mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionMode>))]
    public enum PositionMode
    {
        /// <summary>
        /// ["<c>true</c>"] Dual position mode
        /// </summary>
        [Map("true")]
        DualPositionMode,
        /// <summary>
        /// ["<c>false</c>"] Single position mode
        /// </summary>
        [Map("false")]
        SinglePositionMode
    }
}
