using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Twap order status
    /// </summary>
    public enum TwapOrderStatus
    {
        /// <summary>
        /// New
        /// </summary>
        [Map("New")]
        New,
        /// <summary>
        /// Executing
        /// </summary>
        [Map("Running")]
        Running,
        /// <summary>
        /// Canceling
        /// </summary>
        [Map("Canceling")]
        Canceling,
        /// <summary>
        /// Fully filled
        /// </summary>
        [Map("Filled")]
        Filled,
        /// <summary>
        /// Partially filled
        /// </summary>
        [Map("PartiallyFilled")]
        PartiallyFilled,
        /// <summary>
        /// Pending
        /// </summary>
        [Map("Pending")]
        Pending,
        /// <summary>
        /// Partially filled, remaining order failed
        /// </summary>
        [Map("PartiallyFilledAndResidueFailed")]
        PartiallyFilledAndResidueFailed,
        /// <summary>
        /// Partially filled, remaining cancellation
        /// </summary>
        [Map("PartiallyFilledAndResidueCancelled")]
        PartiallyFilledAndResidueCancelled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("Cancelled")]
        Cancelled,
        /// <summary>
        /// Order failed
        /// </summary>
        [Map("Failed")]
        Failed,
    }
}
