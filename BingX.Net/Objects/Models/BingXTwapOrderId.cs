using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Twap order id
    /// </summary>
    public record BingXTwapOrderId
    {
        /// <summary>
        /// Main order id
        /// </summary>
        [JsonPropertyName("mainOrderId")]
        public long MainOrderId { get; set; }
    }


}
