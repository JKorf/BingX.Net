using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Twap order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TwapOrderStatus>))]
    public enum TwapOrderStatus
    {
        /// <summary>
        /// ["<c>New</c>"] New
        /// </summary>
        [Map("New")]
        New,
        /// <summary>
        /// ["<c>Running</c>"] Executing
        /// </summary>
        [Map("Running")]
        Running,
        /// <summary>
        /// ["<c>Canceling</c>"] Canceling
        /// </summary>
        [Map("Canceling")]
        Canceling,
        /// <summary>
        /// ["<c>Filled</c>"] Fully filled
        /// </summary>
        [Map("Filled")]
        Filled,
        /// <summary>
        /// ["<c>PartiallyFilled</c>"] Partially filled
        /// </summary>
        [Map("PartiallyFilled")]
        PartiallyFilled,
        /// <summary>
        /// ["<c>Pending</c>"] Pending
        /// </summary>
        [Map("Pending")]
        Pending,
        /// <summary>
        /// ["<c>PartiallyFilledAndResidueFailed</c>"] Partially filled, remaining order failed
        /// </summary>
        [Map("PartiallyFilledAndResidueFailed")]
        PartiallyFilledAndResidueFailed,
        /// <summary>
        /// ["<c>PartiallyFilledAndResidueCancelled</c>"] Partially filled, remaining cancellation
        /// </summary>
        [Map("PartiallyFilledAndResidueCancelled")]
        PartiallyFilledAndResidueCancelled,
        /// <summary>
        /// ["<c>Cancelled</c>"] Canceled
        /// </summary>
        [Map("Cancelled")]
        Cancelled,
        /// <summary>
        /// ["<c>Failed</c>"] Order failed
        /// </summary>
        [Map("Failed")]
        Failed,
    }
}
