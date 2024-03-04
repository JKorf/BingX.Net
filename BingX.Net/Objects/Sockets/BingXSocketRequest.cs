using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXSocketRequest
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("reqType")]
        [JsonProperty("reqType")]
        public string RequestType { get; set; } = string.Empty;
        [JsonPropertyName("dataType")]
        [JsonProperty("dataType")]
        public string Topic { get; set; } = string.Empty;
    }
}
