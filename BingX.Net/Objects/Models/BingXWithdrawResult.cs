using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Withdraw result
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXWithdrawResult
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("withdrawOrderId")]
        public string? ClientOrderId { get; set; }
    }
}
