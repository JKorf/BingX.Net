using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Order cancelation result
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXCancelAllResult
    {
        /// <summary>
        /// ["<c>success</c>"] Successfully canceled orders
        /// </summary>
        [JsonPropertyName("success")]
        public BingXFuturesOrderDetails[] Success { get; set; } = Array.Empty<BingXFuturesOrderDetails>();
        /// <summary>
        /// ["<c>failed</c>"] Failed order cancelation results
        /// </summary>
        [JsonPropertyName("failed")]
        public BingXFailedCancel[] Failed { get; set; } = Array.Empty<BingXFailedCancel>();
    }

    /// <summary>
    /// Failed cancelation info
    /// </summary>
    public record BingXFailedCancel
    {
        /// <summary>
        /// ["<c>orderId</c>"] Id of order failed to cancel
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>errorCode</c>"] Error code
        /// </summary>
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        /// <summary>
        /// ["<c>errorMessage</c>"] Error message
        /// </summary>
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; } = string.Empty!;
    }
}
