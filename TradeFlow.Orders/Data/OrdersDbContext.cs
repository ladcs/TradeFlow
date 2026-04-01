using Microsoft.EntityFrameworkCore;
using TradeFlow.Orders.models.Orders;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
    {}

    public DbSet<Order> Orders { get; set; }
}