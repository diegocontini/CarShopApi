namespace CarShopApi.Controllers.Dtos;


public record CreateOrUpdateCarDto
{
    public long? Id { get; set; }
    public bool New { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public double Price { get; set; }

    public string Color { get; set; }

    public int Km { get; set; }

    public string Description { get; set; } 
    
    public IList<CreateOrUpdateCarImageDto> Images { get; set; } = new List<CreateOrUpdateCarImageDto>();
};

public record CreateOrUpdateCarImageDto
{
    public long? Id { get; set; }

    public string Url { get; set; }
}