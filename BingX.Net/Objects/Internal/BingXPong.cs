using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal class BingXPong
    {
        [JsonPropertyName("pong")]
        [JsonProperty("pong")]
        public string Pong { get; set; }
        [JsonPropertyName("time")]
        [JsonProperty("time")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
