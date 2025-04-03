using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    [SerializationModel]
    internal class BingXPong
    {
        [JsonPropertyName("pong")]
        public string Pong { get; set; } = string.Empty;
        [JsonPropertyName("time")]
        public string Timestamp { get; set; } = string.Empty;
    }
}
