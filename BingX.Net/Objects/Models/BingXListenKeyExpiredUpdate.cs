using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Listenkey has expired
    /// </summary>
    [SerializationModel]
    public record BingXListenKeyExpiredUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// ["<c>listenKey</c>"] The listen key
        /// </summary>
        [JsonPropertyName("listenKey")]
        public string ListenKey { get; set; } = string.Empty;
    }
}
