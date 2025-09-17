using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShopApi.Data.Mapping;

public class CarMapping : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {

        builder.ToTable("cars");
        builder.HasKey(c => c.Id);
    }
}