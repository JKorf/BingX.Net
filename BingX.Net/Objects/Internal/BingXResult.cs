using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal record BingXResult
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    internal record BingXResult<T> : BingXResult
    {
        [JsonPropertyName("data")]
        [JsonConverter(typeof(ObjectOrArrayConverter))]
        public T? Data { get; set; }
    }
}
