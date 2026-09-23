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

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBingXSocketClientPerpetualFuturesSharedApi :
        ISubscribeTickerSocket,
        ISubscribeTradesSocket,
        ISubscribeBookTickerSocket,
        ISubscribeBalancesSocket,
        ISubscribePositionsSocket,
        ISubscribeFuturesOrdersSocket,
        ISubscribeKlinesSocket
    { }
}
