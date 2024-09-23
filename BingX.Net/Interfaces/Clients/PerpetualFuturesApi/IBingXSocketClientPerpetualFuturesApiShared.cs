using CryptoExchange.Net.SharedApis.Interfaces.Socket;
using CryptoExchange.Net.SharedApis.Interfaces.Socket.Futures;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Perpetual futures socket API usage
    /// </summary>
    public interface IBingXSocketClientPerpetualFuturesApiShared :
        ITickerSocketClient,
        ITradeSocketClient,
        IBookTickerSocketClient,
        IBalanceSocketClient,
        IPositionSocketClient,
        IFuturesOrderSocketClient
    {
    }
}
