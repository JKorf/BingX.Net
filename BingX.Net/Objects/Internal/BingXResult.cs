using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal record BingXResult<T>
    {
        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonProperty("msg")]
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        [JsonProperty("timestamp")]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
