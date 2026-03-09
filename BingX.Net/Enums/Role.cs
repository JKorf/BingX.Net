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
        /// ["<c>maker</c>"] Maker
        /// </summary>
        [Map("maker")]
        Maker,
        /// <summary>
        /// ["<c>taker</c>"] Taker
        /// </summary>
        [Map("taker")]
        Taker
    }
}
