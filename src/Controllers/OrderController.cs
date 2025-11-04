using CarShopApi.Controllers.Dtos;
using CarShopApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarShopApi.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class OrderController (OrderService service) : ControllerBase
{
    private readonly OrderService _service = service;

    [HttpGet("{vendorId}")]
    public async Task<IActionResult> GetOrders(int vendorId)
    {
        var orders =  await _service.GetOrders(vendorId);
        return Ok(orders);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Models.Order), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateOrderDto orderDto)
    {
        return Ok(await _service.CreateOrUpdateAsync(orderDto));
    }

   

}