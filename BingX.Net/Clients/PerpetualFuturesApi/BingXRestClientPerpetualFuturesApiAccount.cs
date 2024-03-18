using BingX.Net.Interfaces.Clients.PerpetualFuturesApi;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    /// <inheritdoc />
    public class BingXRestClientPerpetualFuturesApiAccount : IBingXRestClientPerpetualFuturesApiAccount
    {
        private readonly BingXRestClientPerpetualFuturesApi _baseClient;

        internal BingXRestClientPerpetualFuturesApiAccount(BingXRestClientPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

    }
}
