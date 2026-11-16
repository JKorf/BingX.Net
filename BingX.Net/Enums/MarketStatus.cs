using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Market status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MarketStatus>))]
    public enum MarketStatus
    {
        /// <summary>
        /// ["<c>open</c>"] Open
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// ["<c>close</c>"] Closed
        /// </summary>
        [Map("close")]
        Closed,
        /// <summary>
        /// ["<c>pause</c>"] Paused
        /// </summary>
        [Map("pause")]
        Paused
    }
}
