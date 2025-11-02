using CarShopApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarShopApi.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class OrderController (OrderService service) : Controller
{
    private readonly OrderService _service = service;

    [HttpGet("{vendorId}")]
    public async Task<IActionResult> GetOrders(int vendorId)
    {
        var orders =  await _service.GetOrders(vendorId);
        return Ok(orders);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] Models.Order order)
    {
        var createdOrder = await _service.Create(order);
        return Ok(createdOrder);
    }
    

}