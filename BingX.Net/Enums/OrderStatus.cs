using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        /// <summary>
        /// New order
        /// </summary>
        [Map("NEW")]
        New,
        /// <summary>
        /// Pending
        /// </summary>
        [Map("PENDING")]
        Pending,
        /// <summary>
        /// Partially filled
        /// </summary>
        [Map("PARTIALLY_FILLED")]
        PartiallyFilled,
        /// <summary>
        /// Filled
        /// </summary>
        [Map("FILLED")]
        Filled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("CANCELED", "CANCELLED")]
        Canceled,
        /// <summary>
        /// Failed
        /// </summary>
        [Map("FAILED")]
        Failed
    }
}
