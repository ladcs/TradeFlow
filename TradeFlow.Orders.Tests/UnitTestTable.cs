using Microsoft.EntityFrameworkCore;
using TradeFlow.Orders.Models;

public class OrderConfigurationTests
{
    private OrdersDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        return new OrdersDbContext(options);
    }

    [Trait("Orders", "Implementação tabela Order")]
    [Fact(DisplayName = "Será validado que cria-se as colunas corretamente que não podem ser nulas")]
    public void Order_notNullables()
    {
        // Arrange
        using var context = CreateContext();
        var model = context.Model.FindEntityType(typeof(Order))!;

        // Assert — verifica se as propriedades existem no model
        Assert.NotNull(model.FindProperty("Id"));
        Assert.NotNull(model.FindProperty("Asset"));
        Assert.NotNull(model.FindProperty("Price"));
        Assert.NotNull(model.FindProperty("Quantity"));
    }

    [Trait("Orders", "Implementação tabela Order")]
    [Fact(DisplayName = "Será validado que cria-se as colunas corretamente com o maximo de tamanho")]
    public void Asset_MaxLength10()
    {
        using var context = CreateContext();
        var model = context.Model.FindEntityType(typeof(Order))!;
        var property = model.FindProperty("Asset")!;

        Assert.Equal(10, property.GetMaxLength());
    }

    [Trait("Orders", "Implementação tabela Order")]
    [Fact(DisplayName = "Será validado que cria-se as colunas nas quais podem ser nulas")]
    public void MustBeNullable()
    {
        using var context = CreateContext();
        var model = context.Model.FindEntityType(typeof(Order))!;
        var property = model.FindProperty("DeletedAt")!;

        Assert.True(property.IsNullable);
        property = model.FindProperty("UpdatedAt")!;
        Assert.True(property.IsNullable);
    }
}