using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Margin mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MarginMode>))]
    public enum MarginMode
    {
        /// <summary>
        /// Isolated margin mode
        /// </summary>
        [Map("ISOLATED", "isolated")]
        Isolated,
        /// <summary>
        /// Cross margin mode
        /// </summary>
        [Map("CROSSED", "cross")]
        Cross
    }
}
