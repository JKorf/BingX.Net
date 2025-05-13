using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Account identifier type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountIdentifierType>))]
    public enum AccountIdentifierType
    {
        /// <summary>
        /// Unique id
        /// </summary>
        [Map("1")]
        Uid,
        /// <summary>
        /// Phone number
        /// </summary>
        [Map("2")]
        PhoneNumber,
        /// <summary>
        /// Email
        /// </summary>
        [Map("3")]
        Email
    }
}
