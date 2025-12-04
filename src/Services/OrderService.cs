using System.Diagnostics;
using CarShopApi.Controllers.Dtos;
using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Services;

public class OrderService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<List<Order>> GetOrders(int? vendorId)
    {
        if (vendorId == null)
        {
            return await _db.Orders.Include(e => e.Items).ToListAsync();
        }

        return await _db.Orders.Include(e => e.Items)
            .Where(o => o.VendorId == vendorId)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(long id)
    {
        return await _db.Orders
            .Include(e => e.Items)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Order> CreateAsync(CreateOrUpdateOrderDto orderDto)
    {
        var transact = await _db.Database.BeginTransactionAsync();
        try
        {
            var order = new Order
            {
                CustomerName = orderDto.CustomerName,
                OrderDate = orderDto.OrderDate,
                Total = orderDto.Total,
                VendorId = orderDto.VendorId
            };

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            await ProcessOrderItems(order, orderDto.Items);
            await ProcessComission(order);

            await transact.CommitAsync();
            return order;
        }
        catch
        {
            await transact.RollbackAsync();
            throw;
        }
    }

    public async Task<Order?> UpdateAsync(long id, CreateOrUpdateOrderDto orderDto)
    {
        var transact = await _db.Database.BeginTransactionAsync();
        try
        {
            var existingOrder = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingOrder == null)
            {
                await transact.RollbackAsync();
                return null;
            }

            existingOrder.CustomerName = orderDto.CustomerName;
            existingOrder.OrderDate = orderDto.OrderDate;
            existingOrder.Total = orderDto.Total;
            existingOrder.VendorId = orderDto.VendorId;

            await _db.SaveChangesAsync();
            await ProcessOrderItems(existingOrder, orderDto.Items);
            await ProcessComission(existingOrder);
            await transact.CommitAsync();
            return existingOrder;
        }
        catch
        {
            await transact.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var order = await GetByIdAsync(id);
        if (order == null)
        {
            return false;
        }

        _db.Orders.Remove(order);
        _db.VendorComissions.RemoveRange(_db.VendorComissions.Where(c => c.OrderId == id));
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task ProcessOrderItems(Order order, IList<CreateOrUpdateOrderItemDto> itemDtos)
    {
        foreach (var itemDto in itemDtos)
        {
            if (itemDto.Id.HasValue)
            {
                var existingItem = await _db.OrderItems
                    .FirstOrDefaultAsync(i => i.Id == itemDto.Id && i.OrderId == order.Id);

                if (existingItem != null)
                {
                    existingItem.CarId = itemDto.CarId;
                    existingItem.Price = itemDto.Price;
                    existingItem.Discount = itemDto.Discount;
                    await _db.SaveChangesAsync();
                    continue;
                }
            }

            var newItem = new OrderItem
            {
                CarId = itemDto.CarId,
                Price = itemDto.Price,
                Discount = itemDto.Discount,
                OrderId = order.Id ?? 0
            };

            await _db.OrderItems.AddAsync(newItem);
            await _db.SaveChangesAsync();
        }
    }

    private async Task ProcessComission(Order order)
    {
        var vendor = await _db.Users.FindAsync(order.VendorId);
        decimal comissionAmount = 0;

        if (vendor?.ComissionPerSaleInPercent is > 0)
        {
            var percent = (decimal)vendor.ComissionPerSaleInPercent / 100;
            comissionAmount = percent * order.Total;
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