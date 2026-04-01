namespace TradeFlow.Orders.Dto;

public record QueueConfig(
    string Exchange,
    bool DurableExchange,
    bool AutoDeleteExchange,
    string Queue,
    bool Exclusive,
    bool DurableQueue,
    bool AutoDeleteQueue
);