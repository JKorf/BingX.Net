using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using BingX.Net.Objects.Models;

namespace BingX.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// BingX futures streams
    /// </summary>
    public interface IBingXSocketClientFuturesApi : ISocketApiClient, IDisposable
    {
    }
}
