using System.Net;
using CarShopApi.Controllers.Dtos;
using CarShopApi.Models;
using CarShopApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CarShopApi.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
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

