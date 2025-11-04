using System.Net;
using CarShopApi.Controllers.Dtos;
using CarShopApi.Models;
using CarShopApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController(UserService userService) : Controller
{
    private readonly UserService _userService = userService;

    [HttpPut]
    [ProducesResponseType<User>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] User user)
    {
        var resp = await _userService.CreateOrUpdateAsync(user);

        return Ok(resp);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var resp = await _userService.AuthenticateLogin(dto);
        if (resp == null)
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(resp);
    }
    
    [HttpGet("{id}")]

    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var resp= await _userService.GetAsync(id);
        if (resp == null)
        {
            return NoContent();
        }
        return Ok(resp);
    }
}

