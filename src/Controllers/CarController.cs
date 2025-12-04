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

    [HttpGet]
    [Authorize(Roles = "Admin,Vendor")] 
    [ProducesResponseType(typeof(IEnumerable<Car>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Vendor")]
    [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var car = await _service.GetByIdAsync(id);
        
        if (car == null)
        {
            return NotFound();
        }
        
        return Ok(car);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Car), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateCarDto carDto)
    {
        try
        {
            var createdCar = await _service.CreateAsync(carDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCar.Id }, createdCar);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] CreateOrUpdateCarDto carDto)
    {
        var updatedCar = await _service.UpdateAsync(id, carDto);
        
        if (updatedCar == null)
        {
            return NotFound();
        }
        
        return Ok(updatedCar);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _service.DeleteAsync(id);
        
        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}