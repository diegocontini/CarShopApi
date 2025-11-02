using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Services;

public class ComissionService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public Task<List<VendorComission>> GetAllAsync(long vendorId)
    {
        var comissions = _db.VendorComissions
            .Where(c => c.VendorId == vendorId)
            .ToListAsync();
        return comissions;
    }

}