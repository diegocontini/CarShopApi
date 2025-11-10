using CarShopApi.Services;
using CarShopApi.Controllers.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class CarController(CarService service) : ControllerBase
{
    private readonly CarService _service = service;

    [HttpGet()]
    [Authorize(Roles = "Admin,Vendor")] 
    [ProducesResponseType( typeof(IEnumerable<Car>) ,StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPut()]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType( typeof(Car) ,StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateCarDto carDto)
    {
        return Ok(await _service.CreateOrUpdateAsync(carDto));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
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