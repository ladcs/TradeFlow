using Microsoft.EntityFrameworkCore;
using TradeFlow.Pricing.Models;

public class PricingDbContext : DbContext
{
    public PricingDbContext(DbContextOptions<PricingDbContext> options)
            : base(options)
    {}

    public DbSet<T> T { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PricingDbContext).Assembly);
    }
}