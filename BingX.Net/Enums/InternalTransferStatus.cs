using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Internal transfer status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<InternalTransferStatus>))]
    public enum InternalTransferStatus
    {
        /// <summary>
        /// Pending review
        /// </summary>
        [Map("4")]
        PendingReview,
        /// <summary>
        /// Failed
        /// </summary>
        [Map("5")]
        Failed,
        /// <summary>
        /// Completed
        /// </summary>
        [Map("6")]
        Completed
    }
}
