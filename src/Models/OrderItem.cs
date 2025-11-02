namespace CarShopApi.Models;

public class OrderItem
{
    public long? Id { get; set; }
    public long OrderId { get; set; }
    public long CarId { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    
    public Order Order { get; set; }
}