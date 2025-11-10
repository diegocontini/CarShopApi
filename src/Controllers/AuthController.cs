using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarShopApi.Controllers.Dtos;
using CarShopApi.Services;

namespace CarShopApi.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Vendor")]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly UserService _userService;

    public AuthController(JwtService jwtService, UserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<IActionResult> CreateToken([FromBody] LoginDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        var user = await _userService.AuthenticateLogin(request);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = _jwtService.GenerateToken(user.Username, user.Role.ToString());
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