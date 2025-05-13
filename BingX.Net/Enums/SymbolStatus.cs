using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Status of a symbol
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// Offline
        /// </summary>
        [Map("0")]
        Offline,
        /// <summary>
        /// Online
        /// </summary>
        [Map("1")]
        Online,
        /// <summary>
        /// Pre-open
        /// </summary>
        [Map("5")]
        PreOpen,
        /// <summary>
        /// Trading suspended
        /// </summary>
        [Map("25")]
        Suspended
    }
}
