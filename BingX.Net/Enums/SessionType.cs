using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Session type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SessionType>))]
    public enum SessionType
    {
        /// <summary>
        /// ["<c>REGULAR</c>"] Regular
        /// </summary>
        [Map("REGULAR")]
        Regular,
        /// <summary>
        /// ["<c>NO_TRADING</c>"] No Trading
        /// </summary>
        [Map("NO_TRADING")]
        NoTrading
    }
}
