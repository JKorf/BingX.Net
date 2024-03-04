using Microsoft.Extensions.Logging;
using BingX.Net.Interfaces.Clients.SpotApi;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BingXRestClientSpotApiTrading : IBingXRestClientSpotApiTrading
    {
        private readonly BingXRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal BingXRestClientSpotApiTrading(ILogger logger, BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }
    }
}
