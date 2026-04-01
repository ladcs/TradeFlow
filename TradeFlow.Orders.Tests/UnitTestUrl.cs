using Microsoft.Extensions.Configuration;

namespace TradeFlow.Orders.Tests;

public class TestTableConfig
{
    [Trait("Orders", "Implementação da url")]
    [Fact(DisplayName = "Será validado que cria-se a url apartir dos parametros")]
    public void GetConnectionString_okay()
    {
        // Arrage - cria um IConfiguration fake
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DB_HOST", "localhost" },
                { "DB_PORT", "5432" },
                { "DB_NAME", "tradeflow" },
                { "DB_USER", "postgres" },
                { "DB_PASS", "senha123" }
            })
            .Build();

        var baseUrl = new BaseUrl(config);

        // Act
        var result = baseUrl.GetConnectionString();

        // Assert
        Assert.Equal("Host=localhost;Port=5432;Database=tradeflow;Username=postgres;Password=senha123", result);
    }

    [Trait("Orders", "Implementação da url")]
    [Fact(DisplayName = "Será validado que cria-se a url sem parametros")]
    public void GetConnectionString_without()
    {
        // Arrange — configuration vazia, deve cair nos ?? padrões
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var baseUrl = new BaseUrl(config);

        // Act
        var result = baseUrl.GetConnectionString();

        // Assert
        Assert.Equal("Host=localhost;Port=5432;Database=tradeflow;Username=postgres;Password=", result);
    }

        [Trait("Orders", "Implementação da url")]
    [Fact(DisplayName = "Será validado que cria-se a url apartir dos parametros")]
    public void GetConnectionString_withPasswithOutotherParams()
    {
        // Arrage - cria um IConfiguration fake
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DB_PASS", "senha123" }
            })
            .Build();

        var baseUrl = new BaseUrl(config);

        // Act
        var result = baseUrl.GetConnectionString();

        // Assert
        Assert.Equal("Host=localhost;Port=5432;Database=tradeflow;Username=postgres;Password=senha123", result);
    }
}
