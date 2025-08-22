using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;

namespace BingX.Net.Clients
{
    /// <inheritdoc />
    public class BingXUserClientProvider : IBingXUserClientProvider
    {
        private static ConcurrentDictionary<string, IBingXRestClient> _restClients = new ConcurrentDictionary<string, IBingXRestClient>();
        private static ConcurrentDictionary<string, IBingXSocketClient> _socketClients = new ConcurrentDictionary<string, IBingXSocketClient>();

        private readonly IOptions<BingXRestOptions> _restOptions;
        private readonly IOptions<BingXSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public BingXUserClientProvider(Action<BingXOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BingXUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<BingXRestOptions> restOptions,
            IOptions<BingXSocketOptions> socketOptions)
        {
            _httpClient = httpClient ?? new HttpClient();
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, ApiCredentials credentials, BingXEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IBingXRestClient GetRestClient(string userIdentifier, ApiCredentials? credentials = null, BingXEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client))
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IBingXSocketClient GetSocketClient(string userIdentifier, ApiCredentials? credentials = null, BingXEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client))
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IBingXRestClient CreateRestClient(string userIdentifier, ApiCredentials? credentials, BingXEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new BingXRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IBingXSocketClient CreateSocketClient(string userIdentifier, ApiCredentials? credentials, BingXEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new BingXSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IOptions<BingXRestOptions> SetRestEnvironment(BingXEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new BingXRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<BingXSocketOptions> SetSocketEnvironment(BingXEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new BingXSocketOptions();
            var restOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
