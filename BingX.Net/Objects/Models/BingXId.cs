using Newtonsoft.Json;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Id result
    /// </summary>
    public record BingXId
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
    }
}
