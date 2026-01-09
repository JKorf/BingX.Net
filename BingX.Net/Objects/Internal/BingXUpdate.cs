using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal record BingXUpdate
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("dataType")]
        public string DataType { get; set; } = string.Empty;
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("s")]
        public string? Symbol { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
        [JsonInclude, JsonPropertyName("ts")]
        internal DateTime? TimestampInt { set => Timestamp = value; }
    }

    [SerializationModel]
    internal record BingXUpdate<T> : BingXUpdate
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
