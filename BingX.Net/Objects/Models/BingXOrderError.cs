﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Error result
    /// </summary>
    public record BingXOrderError
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// Error
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; } = string.Empty;
    }
}
