# Copilot Instructions for BingX.Net

This repository is **BingX.Net**, a strongly typed C#/.NET client library for the BingX REST and WebSocket APIs. It is part of the CryptoExchange.Net ecosystem.

When generating code that consumes BingX.Net, follow these conventions:

## Use BingX.Net, not raw HTTP

Never generate `HttpClient` calls to BingX API URLs. Always use `BingXRestClient` or `BingXSocketClient`. This ensures correct request signing, rate limiting, and error handling.

## Client setup

```csharp
using BingX.Net;
using BingX.Net.Clients;

var restClient = new BingXRestClient(options =>
{
    options.ApiCredentials = new BingXCredentials("API_KEY", "API_SECRET");
});
```

For public market data, credentials are not required.

## Result handling

Methods return `WebCallResult<T>` (REST) or `CallResult<T>` (WebSocket). Always check `.Success` before reading `.Data`. The error is on `.Error`.

## API structure

- `restClient.SpotApi.ExchangeData` - public spot market data
- `restClient.SpotApi.Account` - spot balances, transfers, deposits, withdrawals
- `restClient.SpotApi.Trading` - spot orders and OCO
- `restClient.PerpetualFuturesApi.ExchangeData` - public perpetual futures market data
- `restClient.PerpetualFuturesApi.Account` - futures balances, position mode, leverage, margin mode
- `restClient.PerpetualFuturesApi.Trading` - futures orders and positions
- `restClient.SubAccountApi` - sub-account endpoints
- `socketClient.SpotApi` - Spot WebSocket streams
- `socketClient.PerpetualFuturesApi` - Perpetual Futures WebSocket streams

## Symbols

BingX uses hyphenated symbols in this library, for example `BTC-USDT` and `ETH-USDT`.

## Order placement

Let the library auto-generate `clientOrderId`. Do not pass a custom value unless required for an existing operational flow.

## WebSocket pattern

Store the returned `UpdateSubscription` and unsubscribe on shutdown via `socketClient.UnsubscribeAsync(sub.Data)`.

## Cross-exchange

For code that needs to work across multiple exchanges, use `CryptoExchange.Net.SharedApis` interfaces (`ISpotTickerRestClient`, `ISpotOrderRestClient`, etc.) accessed via `.SharedClient` properties.

Successful shared spot and perpetual symbol retrieval populates `SpotSymbolCatalog` / `FuturesSymbolCatalog`. These symbol queries honor `GetSymbolsRequest` filters and return display names plus asset type metadata; preserve that metadata instead of reclassifying symbols from their names in application code.

## Avoid

- Raw `HttpClient` calls to BingX endpoints
- Generic `ApiCredentials` for BingX credentials
- Invented roots such as `FuturesApi` or `SwapApi`; use `PerpetualFuturesApi`
- Synchronous `.Result` / `.Wait()`
- Instantiating clients per request
- Manual ticker polling when a WebSocket subscription fits
- Manual `clientOrderId` values unless required

## Reference

For detailed patterns and pitfalls see `AGENTS.md`, `llms.txt`, `docs/ai-api-map.md`, and `Examples/ai-friendly/`.
