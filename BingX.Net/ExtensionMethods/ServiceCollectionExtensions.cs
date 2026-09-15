using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using System;
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
using CryptoExchange.Net.Interfaces.Clients;
using System.Threading;
using CryptoExchange.Net.SharedApis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services such as the IBingXRestClient and IBingXSocketClient. Configures the services based on the provided configuration.<br />
        /// See <see href="https://github.com/JKorf/BingX.Net/blob/main/Examples/example-config.json" /> for an example of how to set up the configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddBingX(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = BingXOptions.CreateFromConfiguration(configuration);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

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
            var options = BingXOptions.Create(optionsDelegate);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

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
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(IBingXSocketClient), x => { return new BingXSocketClient(x.GetRequiredService<IOptions<BingXSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<IBingXOrderBookFactory, BingXOrderBookFactory>();
            services.AddTransient<IBingXTrackerFactory, BingXTrackerFactory>();
            services.AddTransient<ITrackerFactory, BingXTrackerFactory>();
            services.AddSingleton<IBingXUserClientProvider, BingXUserClientProvider>(x =>
            new BingXUserClientProvider(
                x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(IBingXRestClient).Name),
                x.GetRequiredService<ILoggerFactory>(),
                x.GetRequiredService<IOptions<BingXRestOptions>>(),
                x.GetRequiredService<IOptions<BingXSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IBingXRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IBingXSocketClient>().SpotApi.SharedClient);
            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IBingXRestClient>().PerpetualFuturesApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IBingXSocketClient>().PerpetualFuturesApi.SharedClient);

            services.RegisterSharedApiClient<
                IBingXSharedApiClient,
                BingXSharedApiClient>(sharedApis => sharedApis
                    .Add(client => client.SpotRest)
                    .Add(client => client.SpotSocket)
                    .Add(client => client.PerpetualFuturesSocket)
                    .Add(client => client.PerpetualFuturesRest));

            return services;
        }
    }
}
