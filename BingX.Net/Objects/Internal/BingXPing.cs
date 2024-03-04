using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Internal
{
    internal class BingXPing
    {
        [JsonProperty("ping")]
        public string Ping { get; set; }
        [JsonProperty("time")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
