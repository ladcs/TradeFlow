using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeFlow.Orders.Models;

namespace TradeFlow.Orders.TableConfigurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        entity.ToTable("Orders");
        entity.HasKey(s => s.Id);

        entity.Property(s => s.CustomerId)
        .IsRequired();

        entity.Property(s => s.Asset)
        .IsRequired()
        .HasMaxLength(10);

        entity.Property(s => s.Type)
        .IsRequired();

        entity.Property(s => s.Quantity)
        .IsRequired()
        .HasColumnType("decimal(18,8)");

        entity.Property(s => s.Price)
        .IsRequired()
        .HasColumnType("decimal(18,2)");

        entity.Property(s => s.Status)
        .IsRequired();

        entity.Property(s => s.CreatedAt)
        .IsRequired()
        .HasDefaultValueSql("NOW()");
    }
}