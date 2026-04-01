using RabbitMQ.Client;
using Testcontainers.RabbitMq;
using TradeFlow.Orders.Dto;
using TradeFlow.RabbitMq.setup;

public class RabbitMqSetupTests : IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitmq = new RabbitMqBuilder("rabbitmq:3-management")
        .WithUsername("guest")
        .WithPassword("guest")
        .Build();

    public async Task InitializeAsync() => await _rabbitmq.StartAsync();
    public async Task DisposeAsync() => await _rabbitmq.DisposeAsync();

    [Trait("Orders", "Implementação conexão com o RabbitMQ")]
    [Fact(DisplayName = "Deve conectar com o RabbitMQ")]
    public async Task DeveConectarNoRabbitMQ()
    {
        // Arrange
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_rabbitmq.GetConnectionString())
        };

        // Act
        var connection = await factory.CreateConnectionAsync();

        // Assert
        Assert.True(connection.IsOpen);

        await connection.CloseAsync();
    }

    [Trait("Orders", "Implementação conexão com o RabbitMQ")]
    [Fact(DisplayName = "Deve criar Exchange e fila")]
    public async Task DeveCriarExchangeEFila()
    {
        // Arrange
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_rabbitmq.GetConnectionString())
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        var config = new QueueConfig(
            Exchange: "orders",
            DurableExchange: true,
            AutoDeleteExchange: false,
            Queue: "orders.processing",
            Exclusive: false,
            DurableQueue: true,
            AutoDeleteQueue: false
        );

        // Act — se não lançar exception, exchange e fila foram criadas
        await RabbitMqSetup.ConfigureAsync(channel, config);

        // Assert — verifica se a fila existe checando o message count
        var result = await channel.QueueDeclarePassiveAsync("orders.processing");
        Assert.NotNull(result);

        await connection.CloseAsync();
    }
}