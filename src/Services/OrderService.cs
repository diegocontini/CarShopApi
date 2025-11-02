using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Services;

public class OrderService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<List<Order>> GetOrders(int vendorId)
    {
        return await _db.Orders
            .Where(o => o.VendorId == vendorId)
            .ToListAsync();
    }

    public async Task<Order> Create(Order order)
    {
        await _db.Orders.AddAsync(order);
        await _db.OrderItems.AddRangeAsync(order.Items);
        await _db.SaveChangesAsync();
        return order;
    }

    private async Task ProcessComission(Order order)
    {
        var vendor = await _db.Users.FindAsync(order.VendorId);
        decimal comissionAmount = 0;

        if (vendor?.ComissionPerSaleInPercent is > 0)
        {
            comissionAmount = vendor.ComissionPerSaleInPercent / 100 * order.Total ?? 0;
        }
        
        var comission = new VendorComission
        {
            Id = null,
            VendorId = vendor?.Id ?? 0,
            VendorName = vendor?.Username ?? string.Empty,
            ComissionPercentage = vendor?.ComissionPerSaleInPercent ?? 0,
            ComissionAmount = comissionAmount,
            OrderId = order.Id ?? 0,
            OrderTotal = order?.Total ?? 0,
        };
        await _db.VendorComissions.AddAsync(comission);
        await _db.SaveChangesAsync();
    }
}