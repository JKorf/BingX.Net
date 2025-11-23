using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageConverters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public override async Task<Error?> CheckForErrorResponse(RequestDefinition request, object state, KeyValuePair<string, string[]>[] responseHeaders, Stream responseStream)
        {
            var (error, document) = await GetJsonDocument(responseStream, state).ConfigureAwait(false);
            if (error != null)
                return error;

            int? code = document!.RootElement.TryGetProperty("code", out var codeProp) ? codeProp.GetInt32() : null;
            if (code == 0)
                return null;

            string? msg = document.RootElement.TryGetProperty("msg", out var msgProp) ? msgProp.GetString() : null;
            return new ServerError(code.ToString()!, _errorMapping.GetErrorInfo(code.ToString()!, msg));
        }

        public override async Task<Error> ParseErrorResponse(int httpStatusCode, object state, KeyValuePair<string, string[]>[] responseHeaders, Stream responseStream)
        {
            using var streamReader = new StreamReader(responseStream);
            return new ServerError(ErrorInfo.Unknown with { Message = await streamReader.ReadToEndAsync().ConfigureAwait(false) });
        }
    }
}
