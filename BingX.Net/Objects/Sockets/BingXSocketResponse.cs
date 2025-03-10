using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Sockets
{
    [SerializationModel]
    internal class BingXSocketResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
    }
}
