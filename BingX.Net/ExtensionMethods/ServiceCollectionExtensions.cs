using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using BingX.Net.Clients;
using BingX.Net.Interfaces;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Options;
using BingX.Net.SymbolOrderBooks;
using CryptoExchange.Net;
using BingX.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services such as the IBingXRestClient and IBingXSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddBingX(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new BingXOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            configuration.Bind(options);

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? BingXEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? BingXEnvironment.Live.Name;
            options.Rest.Environment = BingXEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = BingXEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddBingXCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IBingXRestClient and IBingXSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the BingX services</param>
        /// <returns></returns>
        public static IServiceCollection AddBingX(
            this IServiceCollection services,
            Action<BingXOptions>? optionsDelegate = null)
        {
            var options = new BingXOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? BingXEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? BingXEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddBingXCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddBingXCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IBingXRestClient, BingXRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BingXRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new BingXRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<BingXRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<BingXRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options.Proxy, options.HttpKeepAliveInterval);
            });
            services.Add(new ServiceDescriptor(typeof(IBingXSocketClient), x => { return new BingXSocketClient(x.GetRequiredService<IOptions<BingXSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddSingleton<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<IBingXOrderBookFactory, BingXOrderBookFactory>();
            services.AddTransient<IBingXTrackerFactory, BingXTrackerFactory>();
            services.AddTransient<ITrackerFactory, BingXTrackerFactory>();
            services.AddSingleton<IBingXUserClientProvider, BingXUserClientProvider>(x =>
            new BingXUserClientProvider(
                x.GetRequiredService<HttpClient>(),
                x.GetRequiredService<ILoggerFactory>(),
                x.GetRequiredService<IOptions<BingXRestOptions>>(),
                x.GetRequiredService<IOptions<BingXSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IBingXRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IBingXSocketClient>().SpotApi.SharedClient);
            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IBingXRestClient>().PerpetualFuturesApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IBingXSocketClient>().PerpetualFuturesApi.SharedClient);

            return services;
        }
    }
}
