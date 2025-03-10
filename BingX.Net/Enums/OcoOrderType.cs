using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OcoOrderType>))]
    public enum OcoOrderType
    {
        /// <summary>
        /// Oco limit order
        /// </summary>
        [Map("ocoLimit")]
        OcoLimit,
        /// <summary>
        /// Oco stop limit order
        /// </summary>
        [Map("ocoTps")]
        OcoStopLimit
    }
}
