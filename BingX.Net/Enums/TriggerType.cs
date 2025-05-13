using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Trigger price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerType>))]
    public enum TriggerType
    {
        /// <summary>
        /// Mark price
        /// </summary>
        [Map("MARK_PRICE")]
        MarkPrice,
        /// <summary>
        /// Last price
        /// </summary>
        [Map("CONTRACT_PRICE")]
        LastPrice,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("INDEX_PRICE")]
        IndexPrice
    }
}
