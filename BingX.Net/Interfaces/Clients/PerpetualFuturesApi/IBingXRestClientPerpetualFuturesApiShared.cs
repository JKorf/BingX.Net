using CryptoExchange.Net.SharedApis;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Perpetual futures rest API usage
    /// </summary>
    public interface IBingXRestClientPerpetualFuturesApiShared :
        IBalanceRestClient,
        IFuturesTickerRestClient,
        IFuturesSymbolRestClient,
        IFuturesOrderRestClient,
        IKlineRestClient,
        IRecentTradeRestClient,
        //ITradeHistoryRestClient,
        ILeverageRestClient,
        IMarkPriceKlineRestClient,
        IIndexPriceKlineRestClient,
        IOrderBookRestClient,
        IOpenInterestRestClient,
        IFundingRateRestClient,
        IPositionModeRestClient,
        IPositionHistoryRestClient,
        IListenKeyRestClient,
        IFeeRestClient,
        IFuturesOrderClientIdRestClient,
        IFuturesTpSlRestClient,
        IBookTickerRestClient
    {
    }
}
