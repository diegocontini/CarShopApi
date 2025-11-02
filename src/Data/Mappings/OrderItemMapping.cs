using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShopApi.Data.Mappings;

public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        
        builder.ToTable("order_items");
        builder.HasKey(ci => ci.Id);
        
        builder.HasOne(ci => ci.Order)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CarId);
    }
    
}