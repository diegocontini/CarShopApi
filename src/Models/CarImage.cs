using System.Text.Json.Serialization;

namespace CarShopApi.Models;

public class CarImage
{
    public long? Id { get; set; }
    public string Url { get; set; }
    public long? CarId { get; set; }
    public virtual Car Car { get; set; }
}