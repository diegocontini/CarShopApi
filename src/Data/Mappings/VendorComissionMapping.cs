using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShopApi.Data.Mappings;

public class VendorComissionMapping : IEntityTypeConfiguration<VendorComission>
{
    public void Configure(EntityTypeBuilder<VendorComission> builder)
    {
        builder.ToTable("vendor_comissions");
        builder.HasKey(vc => vc.Id);
    }
}