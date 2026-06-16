# AI-Friendly Examples

These examples are optimized for AI coding assistants and quick onboarding. Each file is:

- Compilable - drop into a console project with `dotnet add package JK.BingX.Net` and it builds
- Self-contained - single file, no external setup, no shared helpers
- Heavily commented - explains why each pattern is used
- Idiomatic - follows current BingX.Net patterns

## Files

| File | What it shows |
|---|---|
| `01-spot-quickstart.cs` | Client setup, public ticker, authenticated balances, place limit order, query order status |
| `02-perpetual-futures.cs` | Perpetual futures: set leverage, place market order, get position, close position pattern |
| `03-websocket.cs` | Subscribe to ticker updates and klines with proper teardown |
| `04-multi-exchange.cs` | `CryptoExchange.Net.SharedApis` pattern for exchange-agnostic code |
| `05-error-handling.cs` | `HttpResult` patterns, retry, common error scenarios |

## Running

```bash
dotnet new console -n MyBingXApp
cd MyBingXApp
dotnet add package JK.BingX.Net
# Copy the example .cs file content into Program.cs
# Replace API_KEY / API_SECRET placeholders for private endpoints
dotnet run
```
