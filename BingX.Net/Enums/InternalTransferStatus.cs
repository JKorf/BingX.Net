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
        /// ["<c>4</c>"] Pending review
        /// </summary>
        [Map("4")]
        PendingReview,
        /// <summary>
        /// ["<c>5</c>"] Failed
        /// </summary>
        [Map("5")]
        Failed,
        /// <summary>
        /// ["<c>6</c>"] Completed
        /// </summary>
        [Map("6")]
        Completed
    }
}
