using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Twap order id
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXTwapOrderId
    {
        /// <summary>
        /// ["<c>mainOrderId</c>"] Main order id
        /// </summary>
        [JsonPropertyName("mainOrderId")]
        public long MainOrderId { get; set; }
    }


}
