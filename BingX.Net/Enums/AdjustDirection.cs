using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Direction to adjust in
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AdjustDirection>))]
    public enum AdjustDirection
    {
        /// <summary>
        /// ["<c>1</c>"] Increase
        /// </summary>
        [Map("1")]
        Increase,
        /// <summary>
        /// ["<c>2</c>"] Decrease
        /// </summary>
        [Map("2")]
        Decrease
    }
}
