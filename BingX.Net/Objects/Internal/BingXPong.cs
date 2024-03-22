using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal class BingXPong
    {
        [JsonPropertyName("pong")]
        public string Pong { get; set; } = string.Empty;
        [JsonPropertyName("time")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
