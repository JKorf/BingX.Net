using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BingX.Net.Clients.MessageHandlers
{
    internal class BingXRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BingXExchange._serializerContext);

        public BingXRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not BingXResult bingXResult)
                return null;

            if (bingXResult.Code == 0)
                return null;

            return new ServerError(bingXResult.Code.ToString()!, _errorMapping.GetErrorInfo(bingXResult.Code.ToString()!, bingXResult.Message));
        }

        public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            if (httpStatusCode == 401 || httpStatusCode == 403)
                return new ServerError(new ErrorInfo(ErrorType.Unauthorized, "Unauthorized"));

            using var streamReader = new StreamReader(responseStream);
            return new ServerError(ErrorInfo.Unknown with { Message = await streamReader.ReadToEndAsync().ConfigureAwait(false) });
        }
    }
}
