using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Role
    /// </summary>
    [JsonConverter(typeof(EnumConverter<Role>))]
    public enum Role
    {
        /// <summary>
        /// Maker
        /// </summary>
        [Map("maker")]
        Maker,
        /// <summary>
        /// Taker
        /// </summary>
        [Map("taker")]
        Taker
    }
}
