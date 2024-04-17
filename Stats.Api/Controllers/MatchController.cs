using Microsoft.AspNetCore.Mvc;
using Stats.Api.Services;

namespace Stats.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : Controller
{
    private readonly IDataService _dataService;

    public MatchController(IDataService dataService)
    {
        _dataService = dataService;
    }
    
    [HttpGet("{leagueId}")]
    public async Task<IActionResult> GetMatchesByLeagueId(string leagueId)
    {
        var matches = await _dataService.GetMatchesByLeagueId(leagueId);
        return Ok(matches);
    }
    
    [HttpGet("{leagueId}/{brandId}")]
    public async Task<IActionResult> GetMatchesByBrandId(string leagueId, string brandId)
    {
        var matches = await _dataService.GetMatchesByBrandId(leagueId, brandId);
        return Ok(matches);
    }
    
}