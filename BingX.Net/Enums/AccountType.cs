using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Account type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountType>))]
    public enum AccountType
    {
        /// <summary>
        /// Funding account
        /// </summary>
        [Map("1")]
        Funding,
        /// <summary>
        /// Standard account
        /// </summary>
        [Map("2")]
        Standard,
        /// <summary>
        /// Perpetual account
        /// </summary>
        [Map("3")]
        Perpetual,
        /// <summary>
        /// Spot account
        /// </summary>
        [Map("15")]
        Spot
    }
}
