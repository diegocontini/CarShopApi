using CarShopApi.Models;
using CarShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,Vendor")]
public class ComissionController(ComissionService service) : ControllerBase
{
    private readonly ComissionService _service = service;

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<VendorComission>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComissionsByVendor([FromQuery]long? vendorId)
    {
        var comissions = await _service.GetAllAsync(vendorId);
        return Ok(comissions);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VendorComission), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var comission = await _service.GetByIdAsync(id);
        
        if (comission == null)
        {
            return NotFound();
        }
        
        return Ok(comission);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VendorComission), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] VendorComission comission)
    {
        try
        {
            var createdComission = await _service.CreateAsync(comission);
            return CreatedAtAction(nameof(GetById), new { id = createdComission.Id }, createdComission);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VendorComission), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] VendorComission comission)
    {
        var updatedComission = await _service.UpdateAsync(id, comission);
        
        if (updatedComission == null)
        {
            return NotFound();
        }
        
        return Ok(updatedComission);
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