using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal record BingXListenKey
    {
        [JsonPropertyName("listenKey")]
        public string ListenKey { get; set; } = string.Empty;
    }
}
