using RabbitMQ.Client;
using TradeFlow.Orders.Dto;

namespace TradeFlow.RabbitMq.setup;
public static class RabbitMqSetup
{
    public static async Task ConfigureAsync(IChannel channel, QueueConfig config)
    {
        await channel.ExchangeDeclareAsync(
            exchange: config.Exchange,
            type: ExchangeType.Fanout,
            durable: config.DurableExchange,
            autoDelete: config.AutoDeleteExchange
        );

        await channel.QueueDeclareAsync(
            queue: config.Queue,
            durable: config.DurableQueue,
            exclusive: config.Exclusive,
            autoDelete: config.AutoDeleteQueue
        );

        await channel.QueueBindAsync(config.Queue, config.Exchange, "");
    }
}