using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TradeFlow.Orders.Models;
using TradeFlow.Orders.Enums;

public class DatabaseConnectionTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres;

    public DatabaseConnectionTests()
    {
        var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "tradeflow";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
        var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "senha123";

        _postgres = new PostgreSqlBuilder("postgres:16")
            .WithDatabase(db)
            .WithUsername(user)
            .WithPassword(pass)
            .Build();
    }

    // sobe o container antes dos testes
    public async Task InitializeAsync() => await _postgres.StartAsync();

    // derruba o container depois dos testes
    public async Task DisposeAsync() => await _postgres.DisposeAsync();

    [Trait("Orders", "Implementação conexão com o banco de dados")]
    [Fact(DisplayName = "Deve conectar com o banco e criar a tabela")]

    public async Task ConnectDb()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        // Act
        await using var context = new OrdersDbContext(options);
        await context.Database.MigrateAsync();

        // Assert — se chegou aqui sem exception, a conexão e migration funcionaram
        Assert.True(await context.Database.CanConnectAsync());
    }

    [Trait("Orders", "Implementação conexão com o banco de dados")]
    [Fact(DisplayName = "Deve inserir uma instancia e buscar a mesma")]
    public async Task InsertAndGet()
    {
        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        await using var context = new OrdersDbContext(options);
        await context.Database.MigrateAsync();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            // Arrange
            var order = new Order
            {
                CustomerId = 1,
                Asset = "AAPL",
                Type = OrderType.Buy,
                Quantity = 10,
                Price = 150.00m,
                Status = OrderStatus.Pending
            };

            // Act
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var result = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AAPL", result.Asset);
            Assert.Equal(150.00m, result.Price);
        }
        finally
        {
            // 🔥 SEMPRE executa (mesmo se o teste falhar)
            await transaction.RollbackAsync();
        }
    }
}