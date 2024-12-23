using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Interfaces;

namespace BingX.Net.Interfaces.Clients.Apis
{
    /// <summary>
    /// BingX sub account API endpoints
    /// </summary>
    public interface IBingXRestClientSubAccountApi: IRestApiClient, IDisposable
    {
        /// <summary>
        /// Get the permissions of the API key
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/common/sub-account#Query%20user%20API%20Key%20permissions" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXKeyPermissions>> GetApiKeyPermissionsAsync(CancellationToken ct = default);

    }
}
