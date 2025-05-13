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
        /// Constant
        /// </summary>
        [Map("constant")]
        Constant,
        /// <summary>
        /// Percentage
        /// </summary>
        [Map("percentage")]
        Percentage
    }
}
