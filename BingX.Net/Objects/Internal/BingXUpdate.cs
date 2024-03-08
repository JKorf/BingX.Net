using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    internal record BingXUpdate<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        [JsonPropertyName("dataType")]
        public string DataType { get; set; } = string.Empty;
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
