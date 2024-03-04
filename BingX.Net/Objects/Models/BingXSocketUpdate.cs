using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Socket update
    /// </summary>
    public record BingXSocketUpdate
    {
        /// <summary>
        /// Event name
        /// </summary>
        [JsonProperty("e")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// Event timestamp
        /// </summary>
        [JsonProperty("E")]
        public DateTime EventTime { get; set; }
    }
}
