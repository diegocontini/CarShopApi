using CarShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,Vendor")]
public class ComissionController(ComissionService service) : Controller
{
    private readonly ComissionService _service = service;
    [HttpGet("{vendorId}")]
    public async Task<IActionResult> GetComissions(long vendorId)
    {
        return Ok(await _service.GetAllAsync(vendorId));
    }
    
}