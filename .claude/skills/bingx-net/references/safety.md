# Operational considerations

## Credentials

BingX private endpoints use an API key and HMAC secret. Use placeholders in examples and retrieve real values from environment variables or secret storage. Limit key permissions to the workflow. `SubAccountApi.GetApiKeyPermissionsAsync` inspects the current key's permissions; do not imply this surface supports creating or managing subaccounts.

## Trading and account changes

Example prices and quantities are illustrative. Check symbol rules and account position mode before placing an order. Limit orders can fill immediately; cancellation is not a guarantee against execution.

Perpetual orders require `PositionSide`: `Both` for one-way mode and `Long` or `Short` for hedge mode. Leverage is side-specific in hedge mode. Match close-order parameters to the position mode and endpoint restrictions; do not apply a one-way reduce-only recipe to every hedge-mode position. `ClosePositionAsync` accepts a position ID; `CloseAllPositionsAsync` has broader scope. Inspect nested results in batch or OCO responses.

## Retry decisions

Use `Error.IsTransient` to identify retry candidates, with bounded backoff and cancellation. A timeout during order submission can leave the outcome unknown. Reconcile the order state before resubmitting; a transient error alone does not make an account-changing request safe to repeat.

See [usage](usage.md) for client and result patterns and [API surfaces](api-surfaces.md) for order lookup and listen-key lifecycle methods.
