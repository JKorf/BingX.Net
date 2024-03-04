using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal record BingXUpdate<T>
    {
        [JsonPropertyName("code")]
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonPropertyName("data")]
        [JsonProperty("data")]
        public T? Data { get; set; }
        [JsonPropertyName("dataType")]
        [JsonProperty("dataType")]
        public string DataType { get; set; } = string.Empty;
        [JsonPropertyName("success")]
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
