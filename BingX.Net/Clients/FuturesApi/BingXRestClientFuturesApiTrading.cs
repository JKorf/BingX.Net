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
