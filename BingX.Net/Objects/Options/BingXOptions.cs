using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Configuration;
using System;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// BingX services options
    /// </summary>
    public class BingXOptions : LibraryOptions<BingXRestOptions, BingXSocketOptions, BingXCredentials, BingXEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();

        /// <summary>
        /// Create options using the provided configuration action
        /// </summary>
        public static BingXOptions Create(Action<BingXOptions>? configure = null)
        {
            var options = CreateUnconfigured();
            configure?.Invoke(options);
            return Normalize(options);
        }

        /// <summary>
        /// Create options using the provided configuration
        /// </summary>
        public static BingXOptions CreateFromConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var options = CreateUnconfigured();
            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid BingX configuration provided", ex);
            }

            if (options.Rest?.Environment != null)
                options.Rest.Environment = BingXEnvironment.GetEnvironmentByName(options.Rest.Environment.Name) ?? options.Rest.Environment;
            if (options.Socket?.Environment != null)
                options.Socket.Environment = BingXEnvironment.GetEnvironmentByName(options.Socket.Environment.Name) ?? options.Socket.Environment;
            if (options.Environment != null)
                options.Environment = BingXEnvironment.GetEnvironmentByName(options.Environment.Name) ?? options.Environment;

            return Normalize(options);
        }

        private static BingXOptions CreateUnconfigured()
        {
            var options = new BingXOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            return options;
        }

        private static BingXOptions Normalize(BingXOptions options)
        {
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment ??= options.Environment ?? BingXEnvironment.Live;
            options.Rest.ApiCredentials ??= options.ApiCredentials;
            options.Socket.Environment ??= options.Environment ?? BingXEnvironment.Live;
            options.Socket.ApiCredentials ??= options.ApiCredentials;
            return options;
        }
    }
}
