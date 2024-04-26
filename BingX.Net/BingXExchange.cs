namespace BingX.Net
{
    /// <summary>
    /// BingX exchange information and configuration
    /// </summary>
    public static class BingXExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "BingX";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.bingx.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://bingx-api.github.io/docs"
            };
    }
}
