namespace CarShopApi.Models;

public class VendorComission
{
    public long? Id { get; set; }
    public long VendorId { get; set; }
    public string VendorName { get; set; }
    public decimal ComissionPercentage { get; set; }
    public decimal ComissionAmount { get; set; }
    public long OrderId { get; set; }
    public decimal OrderTotal { get; set; }
}