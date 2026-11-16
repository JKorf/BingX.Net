# BingX.Net API surfaces

Use this reference to route an intent to the relevant member. Table entries abbreviate overloads; confirm parameters in the installed package before use. For setup and result examples, see [usage](usage.md).

## Client roots

| Surface | Purpose |
| --- | --- |
| `BingXRestClient.SpotApi.ExchangeData` | Spot symbols, tickers, klines, trades, and order books |
| `BingXRestClient.SpotApi.Account` | Spot balances, wallet operations, transfers, and user streams |
| `BingXRestClient.SpotApi.Trading` | Spot orders, OCO, and user trades |
| `BingXRestClient.PerpetualFuturesApi.ExchangeData` | Perpetual market data, funding, and mark prices |
| `BingXRestClient.PerpetualFuturesApi.Account` | Perpetual balances, leverage, margin mode, position mode, and user streams |
| `BingXRestClient.PerpetualFuturesApi.Trading` | Perpetual orders, positions, and TWAP |
| `BingXRestClient.SubAccountApi` | `GetApiKeyPermissionsAsync` for key permissions |
| `BingXSocketClient.SpotApi` | Spot subscriptions |
| `BingXSocketClient.PerpetualFuturesApi` | Perpetual futures subscriptions |

The tables use `client` for a `BingXRestClient` and `socketClient` for a `BingXSocketClient`. Native symbols use hyphens, such as `BTC-USDT`. There is no `FuturesApi` or `SwapApi` root.

## Spot REST

| User intent | BingX.Net member |
|---|---|
| Get server time | `client.SpotApi.ExchangeData.GetServerTimeAsync()` |
| Get spot symbols | `client.SpotApi.ExchangeData.GetSymbolsAsync()` |
| Get one spot symbol | `client.SpotApi.ExchangeData.GetSymbolsAsync("BTC-USDT")` |
| Get latest spot ticker | `client.SpotApi.ExchangeData.GetTickersAsync("BTC-USDT")` |
| Get all spot tickers | `client.SpotApi.ExchangeData.GetTickersAsync()` |
| Get spot last trade price | `client.SpotApi.ExchangeData.GetLastTradeAsync("BTC-USDT")` |
| Get all spot last trade prices | `client.SpotApi.ExchangeData.GetLastTradesAsync()` |
| Get spot order book | `client.SpotApi.ExchangeData.GetOrderBookAsync("BTC-USDT")` |
| Get aggregated spot order book | `client.SpotApi.ExchangeData.GetAggregatedOrderBookAsync("BTC-USDT", limit, mergeDepth)` |
| Get recent trades | `client.SpotApi.ExchangeData.GetRecentTradesAsync("BTC-USDT")` |
| Get historical trades | `client.SpotApi.ExchangeData.GetTradeHistoryAsync("BTC-USDT")` |
| Get klines/candles | `client.SpotApi.ExchangeData.GetKlinesAsync("BTC-USDT", KlineInterval.OneMinute)` |
| Get historical klines | `client.SpotApi.ExchangeData.GetHistoricalKlinesAsync("BTC-USDT", KlineInterval.OneMinute)` |
| Get book price | `client.SpotApi.ExchangeData.GetBookPriceAsync("BTC-USDT")` |
| Get spot balances | `client.SpotApi.Account.GetBalancesAsync()` |
| Get funding balances | `client.SpotApi.Account.GetFundingBalancesAsync()` |
| Get deposit history | `client.SpotApi.Account.GetDepositHistoryAsync(...)` |
| Get withdrawal history | `client.SpotApi.Account.GetWithdrawalHistoryAsync(...)` |
| Get assets | `client.SpotApi.Account.GetAssetsAsync()` |
| Withdraw asset | `client.SpotApi.Account.WithdrawAsync(...)` |
| Get deposit address | `client.SpotApi.Account.GetDepositAddressAsync(asset)` |
| Transfer assets | `client.SpotApi.Account.TransferAsync(...)` |
| Get transfer history | `client.SpotApi.Account.GetTransfersAsync(...)` |
| Internal transfer | `client.SpotApi.Account.TransferInternalAsync(...)` |
| Get internal transfer history | `client.SpotApi.Account.GetInternalTransfersAsync(...)` |
| Start spot user stream | `client.SpotApi.Account.StartUserStreamAsync()` |
| Keep alive spot user stream | `client.SpotApi.Account.KeepAliveUserStreamAsync(listenKey)` |
| Stop spot user stream | `client.SpotApi.Account.StopUserStreamAsync(listenKey)` |
| Get spot trading fees | `client.SpotApi.Account.GetTradingFeesAsync("BTC-USDT")` |
| Get user id | `client.SpotApi.Account.GetUserIdAsync()` |
| Get API key permissions | `client.SpotApi.Account.GetApiKeyPermissionsAsync(userId)` |
| Place spot order | `client.SpotApi.Trading.PlaceOrderAsync(...)` |
| Place multiple spot orders | `client.SpotApi.Trading.PlaceMultipleOrdersAsync(...)` |
| Query spot order | `client.SpotApi.Trading.GetOrderAsync(symbol, orderId)` |
| Get open spot orders | `client.SpotApi.Trading.GetOpenOrdersAsync(symbol)` |
| Get spot order history | `client.SpotApi.Trading.GetOrdersAsync(...)` |
| Cancel spot order | `client.SpotApi.Trading.CancelOrderAsync(symbol, orderId)` |
| Cancel multiple spot orders | `client.SpotApi.Trading.CancelOrdersAsync(...)` |
| Cancel all spot orders | `client.SpotApi.Trading.CancelAllOrdersAsync(symbol)` |
| Dead man's switch | `client.SpotApi.Trading.CancelAllOrdersAfterAsync(...)` |
| Get spot user trades | `client.SpotApi.Trading.GetUserTradesAsync(symbol)` |
| Place OCO order | `client.SpotApi.Trading.PlaceOcoOrderAsync(...)` |
| Cancel OCO order | `client.SpotApi.Trading.CancelOcoOrderAsync(...)` |
| Get OCO order | `client.SpotApi.Trading.GetOcoOrderAsync(...)` |
| Get open OCO orders | `client.SpotApi.Trading.GetOpenOcoOrdersAsync(page, pageSize)` |

## Perpetual Futures REST

| User intent | BingX.Net member |
|---|---|
| Get futures server time | `client.PerpetualFuturesApi.ExchangeData.GetServerTimeAsync()` |
| Get contracts | `client.PerpetualFuturesApi.ExchangeData.GetContractsAsync()` |
| Get one contract | `client.PerpetualFuturesApi.ExchangeData.GetContractsAsync("ETH-USDT")` |
| Get futures ticker | `client.PerpetualFuturesApi.ExchangeData.GetTickerAsync("ETH-USDT")` |
| Get all futures tickers | `client.PerpetualFuturesApi.ExchangeData.GetTickersAsync()` |
| Get futures last trade price | `client.PerpetualFuturesApi.ExchangeData.GetLastTradePriceAsync("ETH-USDT")` |
| Get all futures last trade prices | `client.PerpetualFuturesApi.ExchangeData.GetLastTradePricesAsync()` |
| Get futures order book | `client.PerpetualFuturesApi.ExchangeData.GetOrderBookAsync("ETH-USDT")` |
| Get recent futures trades | `client.PerpetualFuturesApi.ExchangeData.GetRecentTradesAsync("ETH-USDT")` |
| Get futures trade history | `client.PerpetualFuturesApi.ExchangeData.GetTradeHistoryAsync("ETH-USDT")` |
| Get futures klines | `client.PerpetualFuturesApi.ExchangeData.GetKlinesAsync("ETH-USDT", KlineInterval.OneMinute)` |
| Get mark price klines | `client.PerpetualFuturesApi.ExchangeData.GetMarkPriceKlinesAsync("ETH-USDT", KlineInterval.OneMinute)` |
| Get funding rate | `client.PerpetualFuturesApi.ExchangeData.GetFundingRateAsync("ETH-USDT")` |
| Get all funding rates | `client.PerpetualFuturesApi.ExchangeData.GetFundingRatesAsync()` |
| Get funding rate history | `client.PerpetualFuturesApi.ExchangeData.GetFundingRateHistoryAsync("ETH-USDT")` |
| Get open interest | `client.PerpetualFuturesApi.ExchangeData.GetOpenInterestAsync("ETH-USDT")` |
| Get book ticker | `client.PerpetualFuturesApi.ExchangeData.GetBookTickerAsync("ETH-USDT")` |
| Get trading rules | `client.PerpetualFuturesApi.ExchangeData.GetTradingRulesAsync("ETH-USDT")` |
| Get futures balances | `client.PerpetualFuturesApi.Account.GetBalancesAsync()` |
| Get income history | `client.PerpetualFuturesApi.Account.GetIncomesAsync(...)` |
| Get futures trading fees | `client.PerpetualFuturesApi.Account.GetTradingFeesAsync()` |
| Start futures user stream | `client.PerpetualFuturesApi.Account.StartUserStreamAsync()` |
| Keep alive futures user stream | `client.PerpetualFuturesApi.Account.KeepAliveUserStreamAsync(listenKey)` |
| Stop futures user stream | `client.PerpetualFuturesApi.Account.StopUserStreamAsync(listenKey)` |
| Get margin mode | `client.PerpetualFuturesApi.Account.GetMarginModeAsync(symbol)` |
| Set margin mode | `client.PerpetualFuturesApi.Account.SetMarginModeAsync(symbol, MarginMode.Isolated)` |
| Get leverage | `client.PerpetualFuturesApi.Account.GetLeverageAsync(symbol)` |
| Set leverage | `client.PerpetualFuturesApi.Account.SetLeverageAsync(symbol, PositionSide.Long, leverage)` |
| Adjust isolated margin | `client.PerpetualFuturesApi.Account.AdjustIsolatedMarginAsync(...)` |
| Get position mode | `client.PerpetualFuturesApi.Account.GetPositionModeAsync()` |
| Set position mode | `client.PerpetualFuturesApi.Account.SetPositionModeAsync(...)` |
| Get isolated margin history | `client.PerpetualFuturesApi.Account.GetIsolatedMarginChangeHistoryAsync(...)` |
| Apply for VST assets | `client.PerpetualFuturesApi.Account.ApplyForVSTAssetsAsync()` |
| Get multi-asset mode | `client.PerpetualFuturesApi.Account.GetMultiAssetModeAsync()` |
| Set multi-asset mode | `client.PerpetualFuturesApi.Account.SetMultiAssetModeAsync(...)` |
| Get multi-asset rules | `client.PerpetualFuturesApi.Account.GetMultiAssetRulesAsync()` |
| Get multi-assets margin | `client.PerpetualFuturesApi.Account.GetMultiAssetsMarginAsync()` |
| Get positions | `client.PerpetualFuturesApi.Trading.GetPositionsAsync(symbol)` |
| Place test futures order | `client.PerpetualFuturesApi.Trading.PlaceTestOrderAsync(...)` |
| Place futures order | `client.PerpetualFuturesApi.Trading.PlaceOrderAsync(...)` |
| Place multiple futures orders | `client.PerpetualFuturesApi.Trading.PlaceMultipleOrderAsync(...)` |
| Edit futures order | `client.PerpetualFuturesApi.Trading.EditOrderAsync(...)` |
| Query futures order | `client.PerpetualFuturesApi.Trading.GetOrderAsync(symbol, orderId)` |
| Cancel futures order | `client.PerpetualFuturesApi.Trading.CancelOrderAsync(symbol, orderId)` |
| Cancel multiple futures orders | `client.PerpetualFuturesApi.Trading.CancelMultipleOrderAsync(...)` |
| Cancel all futures orders | `client.PerpetualFuturesApi.Trading.CancelAllOrderAsync(...)` |
| Get open futures orders | `client.PerpetualFuturesApi.Trading.GetOpenOrdersAsync(symbol)` |
| Get closed futures orders | `client.PerpetualFuturesApi.Trading.GetClosedOrdersAsync(...)` |
| Get all futures orders | `client.PerpetualFuturesApi.Trading.GetOrdersAsync(...)` |
| Get liquidation orders | `client.PerpetualFuturesApi.Trading.GetLiquidationOrdersAsync(...)` |
| Get futures user trades | `client.PerpetualFuturesApi.Trading.GetUserTradesAsync(...)` |
| Close all positions | `client.PerpetualFuturesApi.Trading.CloseAllPositionsAsync(symbol)` |
| Close position by id | `client.PerpetualFuturesApi.Trading.ClosePositionAsync(positionId)` |
| Get position and margin info | `client.PerpetualFuturesApi.Trading.GetPositionAndMarginInfoAsync(symbol)` |
| Get position history | `client.PerpetualFuturesApi.Trading.GetPositionHistoryAsync(symbol)` |
| Place TWAP order | `client.PerpetualFuturesApi.Trading.PlaceTwapOrderAsync(...)` |
| Get open TWAP orders | `client.PerpetualFuturesApi.Trading.GetOpenTwapOrdersAsync(symbol)` |
| Get closed TWAP orders | `client.PerpetualFuturesApi.Trading.GetClosedTwapOrdersAsync(...)` |
| Cancel TWAP order | `client.PerpetualFuturesApi.Trading.CancelTwapOrderAsync(orderId)` |

## Spot WebSocket

| User intent | BingX.Net member |
|---|---|
| Subscribe spot ticker updates | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(symbol, handler)` |
| Subscribe spot price updates | `socketClient.SpotApi.SubscribeToPriceUpdatesAsync(symbol, handler)` |
| Subscribe spot book price updates | `socketClient.SpotApi.SubscribeToBookPriceUpdatesAsync(symbol, handler)` |
| Subscribe spot trades | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync(symbol, handler)` |
| Subscribe spot klines | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync(symbol, interval, handler)` |
| Subscribe spot partial order book | `socketClient.SpotApi.SubscribeToPartialOrderBookUpdatesAsync(symbol, depth, handler)` |
| Subscribe spot incremental order book | `socketClient.SpotApi.SubscribeToIncrementalOrderBookUpdatesAsync(symbol, handler)` |
| Subscribe spot order updates | `socketClient.SpotApi.SubscribeToOrderUpdatesAsync(listenKey, handler)` |
| Subscribe spot balance updates | `socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(listenKey, handler)` |

## Perpetual Futures WebSocket

| User intent | BingX.Net member |
|---|---|
| Subscribe futures ticker updates | `socketClient.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync(symbol, handler)` |
| Subscribe all futures ticker updates | `socketClient.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync(handler)` |
| Subscribe futures price updates | `socketClient.PerpetualFuturesApi.SubscribeToPriceUpdatesAsync(symbol, handler)` |
| Subscribe futures mark price updates | `socketClient.PerpetualFuturesApi.SubscribeToMarkPriceUpdatesAsync(symbol, handler)` |
| Subscribe futures book price updates | `socketClient.PerpetualFuturesApi.SubscribeToBookPriceUpdatesAsync(symbol, handler)` |
| Subscribe futures trades | `socketClient.PerpetualFuturesApi.SubscribeToTradeUpdatesAsync(symbol, handler)` |
| Subscribe futures klines | `socketClient.PerpetualFuturesApi.SubscribeToKlineUpdatesAsync(symbol, interval, handler)` |
| Subscribe futures partial order book | `socketClient.PerpetualFuturesApi.SubscribeToPartialOrderBookUpdatesAsync(symbol, depth, updateInterval, handler)` |
| Subscribe futures incremental order book | `socketClient.PerpetualFuturesApi.SubscribeToIncrementalOrderBookUpdatesAsync(symbol, handler)` |
| Subscribe futures user data | `socketClient.PerpetualFuturesApi.SubscribeToUserDataUpdatesAsync(listenKey, ...)` |

## User stream lifecycle

With socket credentials configured, prefer private subscription overloads without a listen-key argument: the client obtains and renews the key automatically. Spot exposes `SubscribeToOrderUpdatesAsync(handler)` and `SubscribeToBalanceUpdatesAsync(handler)`; perpetual futures exposes `SubscribeToUserDataUpdatesAsync(...)` with callback arguments.

For an externally managed key, call `StartUserStreamAsync` through the matching REST account API and pass the key to the explicit-key subscription overload. Keep it alive with `KeepAliveUserStreamAsync` and stop it with `StopUserStreamAsync` when no longer needed. Keep spot and perpetual keys with their corresponding APIs. In either mode, unsubscribe through the concrete socket client on shutdown.

## Shared API V2

For portable code, use `.SharedApi` on the selected native REST or socket root. Keep native request/model types separate from Shared API types.

| Intent | Capability or member |
| --- | --- |
| Get a ticker | `IGetTickerRest.GetTickerAsync` |
| Get/filter spot symbols | `IGetSpotSymbolsRest.GetSpotSymbolsAsync` |
| Get/filter futures symbols | `IGetFuturesSymbolsRest.GetFuturesSymbolsAsync` |
| Place a spot order | `IPlaceSpotOrderRest.PlaceSpotOrderAsync` |
| Place a futures order | `IPlaceFuturesOrderRest.PlaceFuturesOrderAsync` |
| Subscribe to tickers | `ISubscribeTickerSocket.SubscribeToTickerUpdatesAsync` |
| Subscribe to trades | `ISubscribeTradesSocket.SubscribeToTradeUpdatesAsync` |
| Subscribe to order books | `ISubscribeOrderBookSocket.SubscribeToOrderBookUpdatesAsync` |

`IBingXSharedApiClient` groups the four supported surfaces:

| Property | Interface |
| --- | --- |
| `SpotRest` | `IBingXRestClientSpotSharedApi` |
| `PerpetualFuturesRest` | `IBingXRestClientPerpetualFuturesSharedApi` |
| `SpotSocket` | `IBingXSocketClientSpotSharedApi` |
| `PerpetualFuturesSocket` | `IBingXSocketClientPerpetualFuturesSharedApi` |

Use a surface property when the operation is known. For dynamic selection, call `GetCapability` on an aggregate instance with a typed descriptor such as `SharedCapabilities.Tickers.GetTicker.Rest`, and specify the intended trading mode when selecting between spot and futures. Check for a null match, then use its `Capability` and `Options`. Use `GetCapabilities` for all matches and `Discover` for summary metadata.

`AddBingX` registers the aggregate and its supported capabilities. Where several surfaces or exchanges implement an injected capability, select the intended implementation explicitly instead of relying on the last DI registration.

### Symbols and quantities

Call `GetSpotSymbolsAsync` or `GetFuturesSymbolsAsync` successfully before reading the corresponding `SpotSymbolCatalog` or `FuturesSymbolCatalog`. `GetSymbolsRequest` supports asset-type filters. Symbol responses populate display names, base/quote asset types, and subtypes. Spot EUR/USD assets are fiat and known stablecoins carry the stablecoin subtype. Perpetual bases starting with `NCSK` classify as TradFi equities and `NCCO` as commodities; other bases classify as crypto. Preserve returned metadata rather than reclassifying symbols in application code.

Shared quantities use `SharedOrderQuantity` metadata, including plural properties such as `Quantities`, `PositionSizes`, and `BestAskQuantities`. Read `QuantityInBaseAsset`, `QuantityInQuoteAsset`, or `QuantityInContracts` as appropriate. BingX shared order books report `QuantityType` as `SharedQuantityType.BaseAsset`.

Shared subscriptions are closed through the concrete `BingXSocketClient.UnsubscribeAsync(subscription.Data)`.

## Source navigation in a checkout

These links are available in a source checkout, not in the standalone NuGet skill:

- [Client interfaces](../../../../BingX.Net/Interfaces/Clients/): exact method signatures and endpoint documentation.
- [Client implementations](../../../../BingX.Net/Clients/): transport and Shared API mappings.
- [Compiled examples](../../../../Examples/ai-friendly/): complete programs.

For packaged usage patterns, see [usage](usage.md).
