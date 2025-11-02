namespace CarShopApi.Models;

public class Order
{
    public long? Id { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public long VendorId { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    
    
}