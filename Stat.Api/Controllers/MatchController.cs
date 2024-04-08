using Microsoft.AspNetCore.Mvc;
using Stat.Api.Services;

namespace Stat.Api.Controllers;

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
        
        // Check if leagues is null or empty
        if (matches == null || !matches.Any())
        {
            return NotFound("No matches found");
        }
        return Ok(matches);
    }
    
    [HttpGet("{leagueId}/{brandId}")]
    public async Task<IActionResult> GetMatchesByBrandId(string leagueId, string brandId)
    {
        var matches = await _dataService.GetMatchesByBrandId(leagueId, brandId);
        
        // Check if leagues is null or empty
        if (matches == null || !matches.Any())
        {
            return NotFound("No matches found");
        }
        return Ok(matches);
    }
    
    
}