using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Cancel replace mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<CancelRestrictions>))]
    public enum CancelRestrictions
    {
        /// <summary>
        /// ["<c>ONLY_NEW</c>"] Only cancel if the order status is New
        /// </summary>
        [Map("ONLY_NEW")]
        OnlyNew,
        /// <summary>
        /// ["<c>ONLY_PENDING</c>"] Only cancel if the order status is Pending
        /// </summary>
        [Map("ONLY_PENDING")]
        OnlyPending,
        /// <summary>
        /// ["<c>ONLY_PARTIALLY_FILLED</c>"] Only cancel if the order status is Partially Filled
        /// </summary>
        [Map("ONLY_PARTIALLY_FILLED")]
        OnlyPartiallyFilled
    }
}
