---
name: bingx-net
description: Build C#/.NET integrations using BingX.Net for BingX spot and perpetual futures REST, WebSockets, authentication, account operations, trading, and Shared APIs. Use when writing or updating code that consumes the library.
---

# BingX.Net

Use `BingXRestClient` and `BingXSocketClient` for BingX exchange access. They provide signing, rate limiting, result handling, and subscription management.

## Setup

```sh
dotnet add package JK.BingX.Net
```

Use the API provided by the installed package. Native client roots are `SpotApi`, `PerpetualFuturesApi`, and REST-only `SubAccountApi`; there is no `FuturesApi` or `SwapApi` root.

## Consumer conventions

- Public market data needs no credentials. Private calls use `BingXCredentials` with an API key and secret; see [authentication](references/usage.md#authentication).
- Use hyphenated native symbols such as `BTC-USDT` and `ETH-USDT`.
- Spot orders use `OrderType`; perpetual orders use `FuturesOrderType` and require `PositionSide`. Use `Both` for one-way mode and `Long` or `Short` for hedge mode.
- Native REST calls return `HttpResult<T>` or `HttpResult`. WebSocket subscriptions return `WebSocketResult<UpdateSubscription>`. Shared non-I/O helpers can return `ExchangeCallResult<T>`. Check `Success` before accessing `Data`.
- Reuse clients, await asynchronous methods, and pass cancellation tokens through the workflow.
- Keep socket callbacks short and unsubscribe through the concrete socket client on shutdown. Private subscriptions use listen keys; see [user streams](references/api-surfaces.md#user-stream-lifecycle).
- Use exchange symbols and trading rules to choose valid quantities and prices. Let the library generate client order IDs unless external correlation is needed.
- For portable multi-exchange code, use the relevant surface's `SharedApi` and narrow capability interfaces. Use `IBingXSharedApiClient` for multiple surfaces or dynamic capability lookup.

## Task references

Read only the references needed for the task:

- [API surfaces](references/api-surfaces.md): Choose a client member, discover Shared API capabilities, or locate symbol catalogs.
- [Usage](references/usage.md): Authentication, results, account reads, trading, subscriptions, and dependency injection examples.
- [Operational considerations](references/safety.md): Key permissions, position modes, ambiguous order outcomes, and retry decisions.
