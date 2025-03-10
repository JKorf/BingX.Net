using BingX.Net.Objects.Models;
using BingX.Net.Enums;
using CryptoExchange.Net.Objects;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BingX Spot trading endpoints, placing and managing orders.
    /// </summary>
    public interface IBingXRestClientSpotApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Place%20order" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="type">Order type</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="price">Order price for limit orders</param>
        /// <param name="quoteQuantity">Order quantity in quote asset</param>
        /// <param name="stopPrice">Stop price</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="timeInForce">Time in force</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, decimal? stopPrice = null, string? clientOrderId = null, TimeInForce? timeInForce = null, CancellationToken ct = default);

        /// <summary>
        /// Place multiple orders
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Place%20multiple%20orders" /></para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="sync">When false (default): Parallel order processing, all orders need to target the same symbol. true: sequential order processing, orders can target different symbols</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrder[]>> PlaceMultipleOrdersAsync(IEnumerable<BingXPlaceOrderRequest> orders, bool? sync = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an active order
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Cancel%20Order" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple active orders
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Cancel%20multiple%20orders" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="orderIds">The order ids to cancel</param>
        /// <param name="clientOrderIds">The client order ids to cancel</param>
        /// <param name="processPartialSuccess">When true the server will handle partially successful requests. When false the full request fails if any of the order cancellations fail.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXCancelsResult>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIds = null, IEnumerable<string>? clientOrderIds = null, bool? processPartialSuccess = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel all active orders on a symbol
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Cancel%20all%20Open%20Orders%20on%20a%20Symbol" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrder[]>> CancelAllOrdersAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Cancel all order after a set period. Can be called contineously to maintain a rolling timeout
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Cancel%20All%20After" /></para>
        /// </summary>
        /// <param name="activate">True to activate the trigger, false to disable the trigger</param>
        /// <param name="cancelAfterSeconds">Seconds after which to cancel all orders, between 10 and 120</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXCancelAfterResult>> CancelAllOrdersAfterAsync(bool activate, int cancelAfterSeconds, CancellationToken ct = default);

        /// <summary>
        /// Get an order by orderId or clientOrderId
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Query%20Order%20details" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrderDetails>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Current%20Open%20Orders" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrderDetails[]>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get order history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Query%20Order%20history" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="type">Filter by type</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXOrderDetails[]>> GetOrdersAsync(string? symbol = null, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trade history
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Query%20transaction%20details" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="type">Filter by type</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="fromId">Return trades after this id</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXUserTrade[]>> GetUserTradesAsync(string symbol, long? orderId = null, OrderStatus? status = null, OrderType? type = null, DateTime? startTime = null, DateTime? endTime = null, long? fromId = null, int? limit = null, CancellationToken ct = default);
		
        /// <summary>
        /// Place a new OCO order
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Create%20an%20OCO%20Order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="limitPrice">Limit order price</param>
        /// <param name="orderPrice">The limit order price set after a stop-limit order is triggered</param>
        /// <param name="triggerPrice">The trigger price of the stop limit order</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="aboveClientOrderId">Client order id for the limit order</param>
        /// <param name="belowClientOrderId">Client order id for the stop limit order</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXOcoOrder[]>> PlaceOcoOrderAsync(string symbol, OrderSide side, decimal quantity, decimal limitPrice, decimal orderPrice, decimal triggerPrice, string? clientOrderId = null, string? aboveClientOrderId = null, string? belowClientOrderId = null, CancellationToken ct = default);
        
        /// <summary>
        /// Cancel an OCO order
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Create%20an%20OCO%20Order" /></para>
        /// </summary>
        /// <param name="orderId">The order id. Either this or `clientOrderId` should be provided</param>
        /// <param name="clientOrderId">The client order id. Either this or `orderId` should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXOrderId>> CancelOcoOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get a specific OCO order
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Query%20an%20OCO%20Order%20List" /></para>
        /// </summary>
        /// <param name="orderListId">The order list id. Either this or `clientOrderId` should be provided</param>
        /// <param name="clientOrderId">The client order id. Either this or `orderListId` should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXOcoOrder[]>> GetOcoOrderAsync(string? orderListId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get open OCO orders
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Query%20All%20Open%20OCO%20Orders" /></para>
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXOcoOrder[]>> GetOpenOcoOrdersAsync(int page, int pageSize, CancellationToken ct = default);

        /// <summary>
        /// Get closed OCO orders
        /// <para><a href="https://bingx-api.github.io/docs/#/en-us/spot/trade-api.html#Query%20All%20Open%20OCO%20Orders" /></para>
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXOcoOrder[]>> GetClosedOcoOrdersAsync(int page, int pageSize, CancellationToken ct = default);
    }
}
