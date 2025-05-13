using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Auto closed order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AutoCloseType>))]
    public enum AutoCloseType
    {
        /// <summary>
        /// Liquidation
        /// </summary>
        [Map("LIQUIDATION")]
        Liquidation,
        /// <summary>
        /// Adl
        /// </summary>
        [Map("ADL")]
        Adl
    }
}
