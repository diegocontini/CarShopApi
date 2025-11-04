namespace CarShopApi.Controllers.Dtos;

public class TokenResponseDto
{
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required string TokenType { get; set; } = "Bearer";
}

public class TokenRequestDto
{
    public required string ApiKey { get; set; }
}

