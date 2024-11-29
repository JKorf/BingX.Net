using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BingX.Net.Interfaces;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;

namespace BingX.Net.SymbolOrderBooks
{
    /// <summary>
    /// BingX order book factory
    /// </summary>
    public class BingXOrderBookFactory : IBingXOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BingXOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Spot = new OrderBookFactory<BingXOrderBookOptions>(CreateSpot, Create);
            PerpetualFutures = new OrderBookFactory<BingXOrderBookOptions>(CreatePerpetualFutures, Create);
        }

        /// <inheritdoc />
        public IOrderBookFactory<BingXOrderBookOptions> Spot { get; }
        /// <inheritdoc />
        public IOrderBookFactory<BingXOrderBookOptions> PerpetualFutures { get; }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<BingXOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(BingXExchange.FormatSymbol);
            if (symbol.TradingMode == TradingMode.Spot)
                return CreateSpot(symbolName, options);

            return CreatePerpetualFutures(symbolName, options);
        }

        /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<BingXOrderBookOptions>? options = null)
            => new BingXSpotSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IBingXRestClient>(),
                                             _serviceProvider.GetRequiredService<IBingXSocketClient>());


        /// <inheritdoc />
        public ISymbolOrderBook CreatePerpetualFutures(string symbol, Action<BingXOrderBookOptions>? options = null)
            => new BingXPerpetualFuturesSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IBingXRestClient>(),
                                             _serviceProvider.GetRequiredService<IBingXSocketClient>());
    }
}
