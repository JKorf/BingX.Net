using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Transfer direction
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TransferDirection>))]
    public enum TransferDirection
    {
        /// <summary>
        /// ["<c>out</c>"] Out
        /// </summary>
        [Map("out")]
        Out,
        /// <summary>
        /// ["<c>in</c>"] In
        /// </summary>
        [Map("in")]
        In
    }
}
