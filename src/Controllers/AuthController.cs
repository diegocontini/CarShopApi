using Microsoft.AspNetCore.Mvc;
using CarShopApi.Services;
using CarShopApi.Controllers.Dtos;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    
    private readonly List<string> _mockedValidApiKeys =
    [
        "123e4567-e89b-12d3-a456-426614174000"
    ];

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("token")]
    public IActionResult CreateToken([FromBody] TokenRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ApiKey))
        {
            return BadRequest(new { message = "ApplicationId is required" });
        }

        if (!_mockedValidApiKeys.Contains(request.ApiKey))
        {
            return BadRequest(new { message = "Invalid API key" });
        }

        var token = _jwtService.GenerateToken(request.ApiKey);
        var expiresAt = _jwtService.GetTokenExpiration();

        var response = new TokenResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            TokenType = "Bearer"
        };

        return Ok(response);
    }
}