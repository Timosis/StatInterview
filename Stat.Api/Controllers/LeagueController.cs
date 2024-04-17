using Microsoft.AspNetCore.Mvc;
using Stat.Api.Services;

namespace Stat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeagueController : ControllerBase
{
    private readonly IDataService _dataService;

    public LeagueController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeagues()
    {
        var leagues = await _dataService.GetLeagues();
        return Ok(leagues);
    }
    
}