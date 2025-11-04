namespace CarShopApi.Controllers.Dtos;

public record CreateOrUpdateOrderDto
{
    public long? Id { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public long VendorId { get; set; }
    public IList<CreateOrUpdateOrderItemDto> Items { get; set; } = new List<CreateOrUpdateOrderItemDto>();
}

public record CreateOrUpdateOrderItemDto
{
    public long? Id { get; set; }
    public long CarId { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}
