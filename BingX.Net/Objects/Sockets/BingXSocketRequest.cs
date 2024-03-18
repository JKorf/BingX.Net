using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXSocketRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("reqType")]
        public string RequestType { get; set; } = string.Empty;
        [JsonPropertyName("dataType")]
        public string Topic { get; set; } = string.Empty;
    }
}
