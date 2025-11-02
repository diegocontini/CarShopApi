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
        await _db.SaveChangesAsync();
        return order;
    }
}