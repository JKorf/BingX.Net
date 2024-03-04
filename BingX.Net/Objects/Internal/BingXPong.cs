using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Internal
{
    internal class BingXPong
    {
        [JsonProperty("pong")]
        public string Pong { get; set; }
        [JsonProperty("time")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
