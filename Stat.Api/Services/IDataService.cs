using Stat.Api.Models;

namespace Stat.Api.Services;

public interface IDataService
{
    Task<ServiceResponse<IEnumerable<League>>> GetLeagues();
    
    Task<ServiceResponse<IEnumerable<Match>>> GetMatchesByLeagueId(string leagueId);
    
    Task<ServiceResponse<IEnumerable<Brand>>> GetBrands();
    
    Task<ServiceResponse<IEnumerable<TeamBrand>>> GetTeamListByBrandId(string brandId);
    
    Task<ServiceResponse<IEnumerable<Match>>> GetMatchesByBrandId(string leagueId,string brandId);
    
}