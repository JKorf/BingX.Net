using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using BingX.Net.Interfaces.Clients.FuturesApi;

namespace BingX.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    public class BingXRestClientFuturesApiAccount : IBingXRestClientFuturesApiAccount
    {
        private readonly BingXRestClientFuturesApi _baseClient;

        internal BingXRestClientFuturesApiAccount(BingXRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

    }
}
