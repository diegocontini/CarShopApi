namespace CarShopApi.Models;

public class CarImage
{
    public long Id { get; set; }

    public string Base64 { get; set; }

    public long CarId { get; set; }

    public Car Car { get; set; }
}