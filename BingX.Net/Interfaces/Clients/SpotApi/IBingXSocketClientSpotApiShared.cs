using CryptoExchange.Net.SharedApis.Interfaces.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    public interface IBingXSocketClientSpotApiShared :
        ITickerSocketClient,
        ITradeSocketClient,
        IBookTickerSocketClient,
        IBalanceSocketClient,
        ISpotOrderSocketClient
    {
    }
}
