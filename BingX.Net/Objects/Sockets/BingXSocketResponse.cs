using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXSocketResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("msg")]
        public string? Message { get; set; }
    }
}
