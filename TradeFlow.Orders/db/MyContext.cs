using Microsoft.EntityFrameworkCore;
using TradeFlow.Orders.models.Orders;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options)
            : base(options)
    {}

    public DbSet<Order> Orders { get; set; }
}