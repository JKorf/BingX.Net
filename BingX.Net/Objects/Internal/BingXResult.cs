using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    [SerializationModel]
    internal record BingXResult<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
