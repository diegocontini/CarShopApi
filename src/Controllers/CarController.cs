using CarShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CarController(CarService service) : ControllerBase
{
    private readonly CarService _service = service;

    [HttpGet("list")]
    [ProducesResponseType( typeof(IEnumerable<Car>) ,StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPut("create-or-update")]
    [ProducesResponseType( typeof(Car) ,StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] Car car)
    {
        return Ok(await _service.CreateOrUpdateAsync(car));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (deleted)
        {
            return Ok();
        }
        return NotFound();
    }
}