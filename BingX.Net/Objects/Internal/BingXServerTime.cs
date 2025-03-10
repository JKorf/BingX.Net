using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    [SerializationModel(typeof(BingXResult<>))]
    internal class BingXServerTime
    {
        [JsonPropertyName("serverTime"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ServerTime { get; set; }
    }
}
