using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Error result
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXOrderError
    {
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>error</c>"] Error
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; } = string.Empty;
    }
}
