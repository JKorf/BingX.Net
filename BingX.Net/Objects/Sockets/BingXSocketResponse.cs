using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXSocketResponse
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("code")]
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        [JsonProperty("msg")]
        public string? Message { get; set; }
    }
}
