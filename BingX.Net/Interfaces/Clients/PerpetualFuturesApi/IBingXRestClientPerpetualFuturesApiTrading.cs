using BingX.Net.Enums;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Interfaces.Clients.PerpetualFuturesApi
{
    /// <summary>
    /// BingX futures trading endpoints, placing and mananging orders.
    /// </summary>
    public interface IBingXRestClientPerpetualFuturesApiTrading
    {
        /// <summary>
        /// Get positions
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/account-api.html#Query%20position%20data" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/user/positions
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXPosition[]>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new test order. Order won't actually get placed
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Test%20Order" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/order/test
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="type">Order type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Limit price</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="reduceOnly">Reduce only</param>
        /// <param name="stopPrice">Stop price</param>
        /// <param name="priceRate">Trailing percentage (between 0 and 1)</param>
        /// <param name="stopLossType">Stop loss order type</param>
        /// <param name="stopLossStopPrice">Stop loss trigger price</param>
        /// <param name="stopLossPrice">Stop loss order price</param>
        /// <param name="stopLossTriggerType">Stop loss trigger price type</param>
        /// <param name="stopLossStopGuaranteed">Stop loss stop guaranteed</param>
        /// <param name="takeProfitType">Take profit order type</param>
        /// <param name="takeProfitStopPrice">Take profit trigger price</param>
        /// <param name="takeProfitPrice">Take profit order price</param>
        /// <param name="takeProfitTriggerType">Take profit trigger price type</param>
        /// <param name="takeProfitStopGuaranteed">Take profit stop guaranteed</param>
        /// <param name="timeInForce">Time in force</param>
        /// <param name="closePosition">Close the position</param>
        /// <param name="triggerPrice">Trigger price</param>
        /// <param name="stopGuaranteed">Stop guaranteed</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrder>> PlaceTestOrderAsync(
            string symbol,
            OrderSide side,
            FuturesOrderType type,
            PositionSide positionSide,
            decimal? quantity = null,
            decimal? price = null,
            bool? reduceOnly = null,
            decimal? stopPrice = null,
            decimal? priceRate = null,

            TakeProfitStopLossMode? stopLossType = null,
            decimal? stopLossStopPrice = null,
            decimal? stopLossPrice = null,
            TriggerType? stopLossTriggerType = null,
            bool? stopLossStopGuaranteed = null,

            TakeProfitStopLossMode? takeProfitType = null,
            decimal? takeProfitStopPrice = null,
            decimal? takeProfitPrice = null,
            TriggerType? takeProfitTriggerType = null,
            bool? takeProfitStopGuaranteed = null,

            TimeInForce? timeInForce = null,
            bool? closePosition = null,
            decimal? triggerPrice = null,
            bool? stopGuaranteed = null,
            string? clientOrderId = null,
            CancellationToken ct = default);


        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Place%20order" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/order
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="type">Order type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Limit price</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="reduceOnly">Reduce only</param>
        /// <param name="stopPrice">Stop price</param>
        /// <param name="priceRate">Trailing percentage (between 0 and 1)</param>
        /// <param name="stopLossType">Stop loss order type</param>
        /// <param name="stopLossStopPrice">Stop loss trigger price</param>
        /// <param name="stopLossPrice">Stop loss order price</param>
        /// <param name="stopLossTriggerType">Stop loss trigger price type</param>
        /// <param name="stopLossStopGuaranteed">Stop loss stop guaranteed</param>
        /// <param name="takeProfitType">Take profit order type</param>
        /// <param name="takeProfitStopPrice">Take profit trigger price</param>
        /// <param name="takeProfitPrice">Take profit order price</param>
        /// <param name="takeProfitTriggerType">Take profit trigger price type</param>
        /// <param name="takeProfitStopGuaranteed">Take profit stop guaranteed</param>
        /// <param name="timeInForce">Time in force</param>
        /// <param name="closePosition">Close the position</param>
        /// <param name="triggerPrice">Trigger price</param>
        /// <param name="stopGuaranteed">Stop guaranteed</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="workingType">Working type for stop orders</param>
        /// <param name="quoteQuantity">Quantity in quote asset</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            FuturesOrderType type,
            PositionSide positionSide,
            decimal? quantity = null,
            decimal? price = null,
            bool? reduceOnly = null,
            decimal? stopPrice = null,
            decimal? priceRate = null,

            TakeProfitStopLossMode? stopLossType = null,
            decimal? stopLossStopPrice = null,
            decimal? stopLossPrice = null,
            TriggerType? stopLossTriggerType = null,
            bool? stopLossStopGuaranteed = null,

            TakeProfitStopLossMode? takeProfitType = null,
            decimal? takeProfitStopPrice = null,
            decimal? takeProfitPrice = null,
            TriggerType? takeProfitTriggerType = null,
            bool? takeProfitStopGuaranteed = null,

            TimeInForce? timeInForce = null,
            bool? closePosition = null,
            decimal? triggerPrice = null,
            bool? stopGuaranteed = null,
            string? clientOrderId = null,
            TriggerType? workingType = null,
            decimal? quoteQuantity = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders in one go
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Place%20multiple%20orders" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/batchOrders
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="sync">When false (default): Parallel order processing, all orders need to target the same symbol. true: sequential order processing, orders can target different symbols</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrder[]>> PlaceMultipleOrderAsync(
            IEnumerable<BingXFuturesPlaceOrderRequest> orders,
            bool? sync = null,
            CancellationToken ct = default);

        /// <summary>
        /// Edit an existing open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Modify%20Order" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v1/trade/amend
        /// </para>
        /// </summary>
        /// <param name="orderId">Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id, either this or orderId should be provided</param>
        /// <param name="symbol">Symbol name</param>
        /// <param name="quantity">New quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXEditResult>> EditOrderAsync(
            long? orderId,
            string? clientOrderId,
            string symbol,
            decimal quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Get an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20pending%20order%20status" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/trade/order
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Order id. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id. Either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrderDetails>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Cancel%20Order" /><br />
        /// Endpoint:<br />
        /// DELETE /openApi/swap/v2/trade/order
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Order id. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id. Either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrderDetails>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Close all positions. Positions will be closed via market order
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Close%20All%20Positions" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/closeAllPositions
        /// </para>
        /// </summary>
        /// <param name="symbol">Only close for a specific symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXClosePositionsResult>> CloseAllPositionsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders on a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Cancel%20multiple%20orders" /><br />
        /// Endpoint:<br />
        /// DELETE /openApi/swap/v2/trade/batchOrders
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="orderIds">The ids of orders to cancel</param>
        /// <param name="clientOrderIds">The client order ids of orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXCancelAllResult>> CancelMultipleOrderAsync(string symbol, IEnumerable<long>? orderIds, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Cancel%20All%20Orders" /><br />
        /// Endpoint:<br />
        /// DELETE /openApi/swap/v2/trade/allOpenOrders
        /// </para>
        /// </summary>
        /// <param name="symbol">Only cancel orders for this symbol</param>
        /// <param name="orderType">Only cancel orders of this type</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXCancelAllResult>> CancelAllOrderAsync(string? symbol = null, OrderType? orderType = null, CancellationToken ct = default);

        /// <summary>
        /// Get all open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Cancel%20All%20Open%20Orders" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/trade/openOrders
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderType">Filter by type of order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrderDetails[]>> GetOpenOrdersAsync(string? symbol = null, OrderType? orderType = null, CancellationToken ct = default);

        /// <summary>
        /// Get liquidation order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#User's%20Force%20Orders" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/trade/forceOrders
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="settleAsset">Filter by settlement asset, USDC or USDT</param>
        /// <param name="closeType">Filter by close type</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrderDetails[]>> GetLiquidationOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get closed orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Order%20history" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/trade/allOrders
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="settleAsset">Filter by settlement asset, USDC or USDT</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesOrderDetails[]>> GetClosedOrdersAsync(string? symbol = null, long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20historical%20transaction%20orders" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v2/trade/allFillOrders
        /// </para>
        /// </summary>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="settleAsset">Filter by settlement asset, USDC or USDT</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesUserTrade[]>> GetUserTradesAsync(long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20historical%20transaction%20details" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/trade/fillHistory
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="settleAsset">Filter by settlement asset, USDC or USDT</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="fromId">Return results after this id</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXFuturesUserTradeDetails[]>> GetUserTradesAsync(string symbol, long? orderId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, long? fromId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel all order after a set period. Can be called contineously to maintain a rolling timeout
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Cancel%20All%20After" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v2/trade/cancelAllAfter
        /// </para>
        /// </summary>
        /// <param name="activate">True to activate the trigger, false to disable the trigger</param>
        /// <param name="cancelAfterSeconds">Seconds after which to cancel all orders, between 10 and 120</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXCancelAfterResult>> CancelAllOrdersAfterAsync(bool activate, int cancelAfterSeconds, CancellationToken ct = default);

        /// <summary>
        /// Close a position by its id
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Close%20position%20by%20position%20ID" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v1/trade/closePosition
        /// </para>
        /// </summary>
        /// <param name="positionId">The id of the position to close</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXClosePositionResult>> ClosePositionAsync(string positionId, CancellationToken ct = default);

        /// <summary>
        /// Get all orders, max 7 days ago
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#All%20Orders" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/trade/fullOrder
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXFuturesOrderDetails[]>> GetOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get position and margin info
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Position%20and%20Maintenance%20Margin%20Ratio" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/maintMarginRatio
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol name, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BingXPositionMarginInfo[]>> GetPositionAndMarginInfoAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get position close history
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20Position%20History" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/trade/positionHistory
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="positionId">Filter by position id</param>
        /// <param name="settleAsset">Filter by settlement asset, USDC or USDT</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXPositionHistory[]>> GetPositionHistoryAsync(string symbol, long? positionId = null, string? settleAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new time weighted average price order
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Place%20TWAP%20Order" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v1/twap/order
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="orderSide">Order side</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="priceType">Price type</param>
        /// <param name="priceVariance">When type is constant, it represents the price difference (unit is USDT), when type is percentage, it represents the slippage ratio (unit is %)</param>
        /// <param name="triggerPrice">Trigger price</param>
        /// <param name="interval">After the strategic order is split, the time interval for order placing is between 5-120s.</param>
        /// <param name="orderQuantity">Maximum quantity for a single order</param>
        /// <param name="totalQuantity">Total quantity to trade</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXTwapOrderId>> PlaceTwapOrderAsync(string symbol, OrderSide orderSide, PositionSide positionSide, PriceType priceType, decimal priceVariance, decimal triggerPrice, int interval, decimal orderQuantity, decimal totalQuantity, CancellationToken ct = default);

        /// <summary>
        /// Get open Twap orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20TWAP%20Entrusted%20Order" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/twap/openOrders
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXTwapOrders>> GetOpenTwapOrdersAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get closed Twap orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20TWAP%20Historical%20Orders" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/twap/historyOrders
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXTwapOrders>> GetClosedTwapOrdersAsync(string symbol, int page, int pageSize, DateTime startTime, DateTime endTime, CancellationToken ct = default);

        /// <summary>
        /// Get a Twap order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Query%20TWAP%20Historical%20Orders" /><br />
        /// Endpoint:<br />
        /// GET /openApi/swap/v1/twap/orderDetail
        /// </para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXTwapOrder>> GetTwapOrderAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel Twap order
        /// <para>
        /// Docs:<br />
        /// <a href="https://bingx-api.github.io/docs/#/en-us/swapV2/trade-api.html#Cancel%20TWAP%20Order" /><br />
        /// Endpoint:<br />
        /// POST /openApi/swap/v1/twap/cancelOrder
        /// </para>
        /// </summary>
        /// <param name="orderId">Main order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BingXTwapOrder>> CancelTwapOrderAsync(long orderId, CancellationToken ct = default);

    }
}
