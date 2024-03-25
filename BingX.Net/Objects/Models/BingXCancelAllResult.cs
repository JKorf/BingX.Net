using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Order cancelation result
    /// </summary>
    public record BingXCancelAllResult
    {
        /// <summary>
        /// Successfully canceled orders
        /// </summary>
        [JsonPropertyName("success")]
        public IEnumerable<BingXFuturesOrderDetails> Success { get; set; } = Array.Empty<BingXFuturesOrderDetails>();
        /// <summary>
        /// Failed order cancelation results
        /// </summary>
        [JsonPropertyName("failed")]
        public IEnumerable<BingXFailedCancel> Failed { get; set; } = Array.Empty<BingXFailedCancel>();
    }

    /// <summary>
    /// Failed cancelation info
    /// </summary>
    public record BingXFailedCancel
    {
        /// <summary>
        /// Id of order failed to cancel
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Error code
        /// </summary>
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; } = string.Empty!;
    }
}
