using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.FuturesApi;

namespace BingX.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    public class BingXRestClientFuturesApiTrading : IBingXRestClientFuturesApiTrading
    {
        private readonly BingXRestClientFuturesApi _baseClient;

        internal BingXRestClientFuturesApiTrading(ILogger logger, BingXRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }
    }
}
