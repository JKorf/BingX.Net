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
