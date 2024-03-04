using BingX.Net.Interfaces.Clients.SpotApi;

namespace BingX.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BingXRestClientSpotApiAccount : IBingXRestClientSpotApiAccount
    {
        private readonly BingXRestClientSpotApi _baseClient;

        internal BingXRestClientSpotApiAccount(BingXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }
    }
}
