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

    [HttpGet]
    [ProducesResponseType<IEnumerable<User>>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.Get();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<User>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await _userService.GetAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType<User>((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        try
        {
            var createdUser = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType<User>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] User user)
    {
        var updatedUser = await _userService.UpdateAsync(id, user);
        
        if (updatedUser == null)
        {
            return NotFound();
        }
        
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _userService.DeleteAsync(id);
        
        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}
