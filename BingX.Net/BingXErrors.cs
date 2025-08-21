using CryptoExchange.Net.Objects.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingX.Net
{
    internal static class BingXErrors
    {
        public static ErrorMapping SpotErrors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.Unauthorized, false, "Signature error", "100001"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Invalid API key", "100413"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Insufficient permissions", "100004"),
                new ErrorInfo(ErrorType.Unauthorized, false, "IP not whitelisted", "100419"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Listen key expired", "100401"),

                new ErrorInfo(ErrorType.InvalidTimestamp, false, "Invalid timestamp", "100421"),

                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", "80014"),
                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid or missing arguments", "100400"),
                new ErrorInfo(ErrorType.InvalidParameter, false, "Maximum leverage exceeded", "101414"),

                new ErrorInfo(ErrorType.InvalidPrice, false, "Order price should be higher than estimated liquidation price", "101460"),
                new ErrorInfo(ErrorType.InvalidPrice, false, "Order price not within range", "101211"),

                new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "PostOnly order could not be placed", "101215"),

                new ErrorInfo(ErrorType.UnknownOrder, false, "Order does not exists", "80016"),

                new ErrorInfo(ErrorType.UnknownSymbol, false, "Symbol not found", "100204"),

                new ErrorInfo(ErrorType.NoPosition, false, "No open position", "80017"),

                new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient margin", "101204"),
                new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "100202"),

                new ErrorInfo(ErrorType.RateLimitOrder, false, "Entrust order limit reached", "80013"),

                new ErrorInfo(ErrorType.InvalidOperation, false, "Order already filled", "80018"),

                new ErrorInfo(ErrorType.RateLimitRequest, false, "Rate limit reached", "100410"),

                new ErrorInfo(ErrorType.UnavailableSymbol, false, "Symbol is not currently trading", "101415"),
            ]
        );

        public static ErrorMapping FuturesErrors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.Unauthorized, false, "Signature error", "100001"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Invalid API key", "100413"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Insufficient permissions", "100004"),
                new ErrorInfo(ErrorType.Unauthorized, false, "IP not whitelisted", "100419"),

                new ErrorInfo(ErrorType.InvalidTimestamp, false, "Invalid timestamp", "100421"),

                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", "80014"),
                new ErrorInfo(ErrorType.InvalidParameter, false, "Maximum leverage exceeded", "101414"),
                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid or missing arguments", "100400"),

                new ErrorInfo(ErrorType.InvalidPrice, false, "Order price should be higher than estimated liquidation price", "101460"),
                new ErrorInfo(ErrorType.InvalidPrice, false, "Order price not within range", "101211"),

                new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "PostOnly order could not be placed", "101215"),

                new ErrorInfo(ErrorType.InvalidOperation, false, "Order already filled", "80018"),

                new ErrorInfo(ErrorType.UnknownOrder, false, "Order does not exists", "80016"),

                new ErrorInfo(ErrorType.NoPosition, false, "No open position", "80017"),
                new ErrorInfo(ErrorType.MaxPosition, false, "Max position value reached", "101209"),

                new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient margin", "101204"),

                new ErrorInfo(ErrorType.RateLimitOrder, false, "Entrust order limit reached", "80013"),

                new ErrorInfo(ErrorType.RateLimitRequest, false, "Rate limit reached", "100410"),

                new ErrorInfo(ErrorType.UnavailableSymbol, false, "Symbol is not currently trading", "101415"),
            ]
        );
    }
}
