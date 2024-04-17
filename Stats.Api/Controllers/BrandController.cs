using Microsoft.AspNetCore.Mvc;
using Stats.Api.Services;

namespace Stats.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : Controller
{
    private readonly IDataService _dataService;

    public BrandController(IDataService dataService)
    {
        _dataService = dataService;
    }
    
    [HttpGet("GetBrandList")]
    public async Task<IActionResult> GetBrandList()
    {
        var brandList = await _dataService.GetBrands();
        return Ok(brandList);
    }
    
    [HttpGet("GetTeamListByBrandId/{brandId}")]
    public async Task<IActionResult> GetTeamListByBrandId(string brandId)
    {
        var teams = await _dataService.GetTeamListByBrandId(brandId);
        return Ok(teams);
    }
    
}