using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal class BingXPing
    {
        [JsonPropertyName("ping")]
        public string Ping { get; set; }
        [JsonPropertyName("time")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
