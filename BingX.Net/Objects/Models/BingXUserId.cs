using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// BingX user id
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXUserId
    {
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("uid")]
        public long UserId { get; set; }
    }
}
