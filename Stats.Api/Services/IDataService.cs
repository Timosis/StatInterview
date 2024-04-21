using Stats.Api.Models;

namespace Stats.Api.Services;

public interface IDataService
{
    Task<ServiceResponse<List<League>>> GetLeagues();
    
    Task<ServiceResponse<List<Match>>> GetMatchesByLeagueId(string leagueId);
    
    Task<ServiceResponse<List<Brand>>> GetBrands();
    
    Task<ServiceResponse<List<TeamBrand>>> GetTeamListByBrandId(string brandId);
    
    Task<ServiceResponse<List<Match>>> GetMatchesByBrandId(string leagueId,string brandId);
    
}