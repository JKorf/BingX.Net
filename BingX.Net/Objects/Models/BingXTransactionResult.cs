using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Transaction result
    /// </summary>
    public record BingXTransactionResult
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("tranId")]
        public string TransactionId { get; set; } = string.Empty;
    }
}
