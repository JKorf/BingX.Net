using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXSocketRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("reqType")]
        public string RequestType { get; set; } = string.Empty;
        [JsonProperty("dataType")]
        public string Topic { get; set; } = string.Empty;
    }
}
