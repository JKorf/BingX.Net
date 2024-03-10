using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Deposit status
    /// </summary>
    public enum DepositStatus
    {
        /// <summary>
        /// In progress
        /// </summary>
        [Map("0")]
        InProgress,
        /// <summary>
        /// Chain uploaded
        /// </summary>
        [Map("6")]
        ChainUploaded,
        /// <summary>
        /// Completed
        /// </summary>
        [Map("1")]
        Completed
    }
}
