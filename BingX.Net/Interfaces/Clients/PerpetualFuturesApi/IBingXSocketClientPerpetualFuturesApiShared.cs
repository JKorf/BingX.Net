using CryptoExchange.Net.SharedApis;

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
        IFuturesOrderSocketClient,
        IKlineSocketClient
    {
    }
}
