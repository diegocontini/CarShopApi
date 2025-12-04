using CarShopApi.Controllers.Dtos;
using CarShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController (OrderService service) : ControllerBase
{
    private readonly OrderService _service = service;

    [HttpGet()]
    [Authorize(Roles = "Admin,Vendor")]
    [ProducesResponseType(typeof(IEnumerable<Models.Order>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersByVendor([FromQuery]int? vendorId)
    {
        var orders = await _service.GetOrders(vendorId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Vendor")]
    [ProducesResponseType(typeof(Models.Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var order = await _service.GetByIdAsync(id);
        
        if (order == null)
        {
            return NotFound();
        }
        
        return Ok(order);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Vendor")]
    [ProducesResponseType(typeof(Models.Order), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateOrderDto orderDto)
    {
        try
        {
            var createdOrder = await _service.CreateAsync(orderDto);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Vendor")]
    [ProducesResponseType(typeof(Models.Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] CreateOrUpdateOrderDto orderDto)
    {
        var updatedOrder = await _service.UpdateAsync(id, orderDto);
        
        if (updatedOrder == null)
        {
            return NotFound();
        }
        
        return Ok(updatedOrder);
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