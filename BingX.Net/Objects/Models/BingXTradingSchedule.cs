using BingX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trading schedule
    /// </summary>
    public record BingXTradingSchedule
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>contractType</c>"] Contract Type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// ["<c>marketStatus</c>"] Market Status
        /// </summary>
        [JsonPropertyName("marketStatus")]
        public MarketStatus MarketStatus { get; set; }
        /// <summary>
        /// ["<c>timeZone</c>"] Time Zone
        /// </summary>
        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>updateTime</c>"] Update Time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>sessions</c>"] Sessions
        /// </summary>
        [JsonPropertyName("sessions")]
        public BingXTradingScheduleSession[] Sessions { get; set; } = [];
    }

    /// <summary>
    /// Trading schedule session
    /// </summary>
    public record BingXTradingScheduleSession
    {
        /// <summary>
        /// ["<c>startTime</c>"] Start Time
        /// </summary>
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// ["<c>endTime</c>"] End Time
        /// </summary>
        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Type
        /// </summary>
        [JsonPropertyName("type")]
        public SessionType Type { get; set; }
    }
}
