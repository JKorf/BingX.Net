using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// BingX services options
    /// </summary>
    public class BingXOptions : LibraryOptions<BingXRestOptions, BingXSocketOptions, ApiCredentials, BingXEnvironment>
    {
    }
}
