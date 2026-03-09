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
        /// ["<c>1</c>"] Funding account
        /// </summary>
        [Map("1")]
        Funding,
        /// <summary>
        /// ["<c>2</c>"] Standard account
        /// </summary>
        [Map("2")]
        Standard,
        /// <summary>
        /// ["<c>3</c>"] Perpetual account
        /// </summary>
        [Map("3")]
        Perpetual,
        /// <summary>
        /// ["<c>15</c>"] Spot account
        /// </summary>
        [Map("15")]
        Spot
    }
}
