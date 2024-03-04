using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Internal
{
    internal record BingXUpdate<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("data")]
        public T? Data { get; set; }
        [JsonProperty("dataType")]
        public string DataType { get; set; } = string.Empty;
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
