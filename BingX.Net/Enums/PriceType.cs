using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PriceType>))]
    public enum PriceType
    {
        /// <summary>
        /// ["<c>constant</c>"] Constant
        /// </summary>
        [Map("constant")]
        Constant,
        /// <summary>
        /// ["<c>percentage</c>"] Percentage
        /// </summary>
        [Map("percentage")]
        Percentage
    }
}
