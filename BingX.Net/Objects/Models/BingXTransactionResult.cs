using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Transaction result
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXTransactionResult
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("tranId")]
        public long TransactionId { get; set; }
    }
}
