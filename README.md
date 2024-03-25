# ![.BingX.Net](https://github.com/JKorf/BingX.Net/blob/781598404ac510d462c91e6888be2d530bd6e350/BingX.Net/Icon/BingX.png) BingX.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/BingX.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/BingX.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/BingX.Net?style=for-the-badge)

BingX.Net is a client library for accessing the [BingX REST and Websocket API](https://bingx-api.github.io/docs/#/en-us/swapV2/changelog). All data is mapped to readable models and enum values. Additional features include an implementation for maintaining a client side order book, easy integration with other exchange client libraries and more.

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Get the library
[![Nuget version](https://img.shields.io/nuget/v/BingX.net.svg?style=for-the-badge)](https://www.nuget.org/packages/BingX.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/BingX.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/BingX.Net)

	dotnet add package BingX.Net
	
## How to use
*REST Endpoints*  

```csharp
// Get the ETH/USDT ticker via rest request
var restClient = new BingXRestClient();
var tickerResult = await restClient.SpotApi.ExchangeData.GetTickersAsync("ETH-USDT");
var lastPrice = tickerResult.Data.Single().LastPrice;
```
	
*Websocket streams*  

```csharp
// Subscribe to ETH/USDT ticker updates via the websocket API
var socketClient = new BingXSocketClient();
var tickerSubscriptionResult = socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETHUSDT", (update) =>
{
    var lastPrice = update.Data.LastPrice;
});
```

For information on the clients, dependency injection, response processing and more see the [documentation](https://jkorf.github.io/BingX.Net/), or have a look at the examples [here](https://github.com/JKorf/BingX.Net/tree/main/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## CryptoExchange.Net
BingX.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://jkorf.github.io/CryptoExchange.Net#idocs_common).

|Exchange|Repository|Nuget|
|--|--|--|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|Huobi/HTX|[JKorf/Huobi.Net](https://github.com/JKorf/Huobi.Net)|[![Nuget version](https://img.shields.io/nuget/v/Huobi.net.svg?style=flat-square)](https://www.nuget.org/packages/Huobi.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### Spot
|API|Supported|Location|
|--|--:|--|
|Market Interface|✓|`restClient.SpotApi.ExchangeData`|
|Wallet deposits and withdrawals|✓|`restClient.SpotApi.Account`|
|Spot account|✓|`restClient.SpotApi.Account`|
|Trade interface|✓|`restClient.SpotApi.Trading`|
|Websocket Market Data|✓|`socketClient.SpotApi`|
|Websocket Account Data|✓|`socketClient.SpotApi`|

### Perpetual Futures
|API|Supported|Location|
|--|--:|--|
|Market Interface|✓|`restClient.PerpetualFuturesApi.ExchangeData`|
|Account Interface|✓|`restClient.PerpetualFuturesApi.Account`|
|Trade Interface|✓|`restClient.PerpetualFuturesApi.Account`/`restClient.PerpetualFuturesApi.Trading`|
|Websocket Market Data|✓|`socketClient.PerpetualFuturesApi`|
|Websocket Account Data|✓|`socketClient.PerpetualFuturesApi`|

### Standard Contract
|API|Supported|Location|
|--|--:|--|
|Standard Contract Interface|X||

### Account & Wallet
|API|Supported|Location|
|--|--:|--|
|Spot account|✓|`restClient.SpotApi.Account`|
|Wallet deposits and withdrawals|✓|`restClient.SpotApi.Account`|
|Sub-account|X||

### Copy Trading
|API|Supported|Location|
|--|--:|--|
|Standard Contract Interface|X||

## Support the project
I develop and maintain this package on my own for free in my spare time, any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1q277a5n54s2l2mzlu778ef7lpkwhjhyvghuv8qf  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 1.0.0 - 25 Mar 2024
    * Initial release