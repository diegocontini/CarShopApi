using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Services;

public class ComissionService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public Task<List<VendorComission>> GetAllAsync(long? vendorId)
    {
        if (vendorId == null)
        {
            return _db.VendorComissions.ToListAsync();
        }
        var comissions = _db.VendorComissions
            .Where(c => c.VendorId == vendorId)
            .ToListAsync();
        return comissions;
    }

    public async Task<VendorComission?> GetByIdAsync(long id)
    {
        return await _db.VendorComissions.FindAsync(id);
    }

    public async Task<VendorComission> CreateAsync(VendorComission comission)
    {
        _db.VendorComissions.Add(comission);
        await _db.SaveChangesAsync();
        return comission;
    }

    public async Task<VendorComission?> UpdateAsync(long id, VendorComission comission)
    {
        var existingComission = await GetByIdAsync(id);
        if (existingComission == null)
        {
            return null;
        }

        existingComission.VendorId = comission.VendorId;
        existingComission.VendorName = comission.VendorName;
        existingComission.ComissionPercentage = comission.ComissionPercentage;
        existingComission.ComissionAmount = comission.ComissionAmount;
        existingComission.OrderId = comission.OrderId;
        existingComission.OrderTotal = comission.OrderTotal;

        await _db.SaveChangesAsync();
        return existingComission;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var comission = await GetByIdAsync(id);
        if (comission == null)
        {
            return false;
        }

        _db.VendorComissions.Remove(comission);
        await _db.SaveChangesAsync();
        return true;
    }
}