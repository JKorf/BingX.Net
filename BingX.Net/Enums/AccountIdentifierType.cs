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
        /// ["<c>1</c>"] Unique id
        /// </summary>
        [Map("1")]
        Uid,
        /// <summary>
        /// ["<c>2</c>"] Phone number
        /// </summary>
        [Map("2")]
        PhoneNumber,
        /// <summary>
        /// ["<c>3</c>"] Email
        /// </summary>
        [Map("3")]
        Email
    }
}
