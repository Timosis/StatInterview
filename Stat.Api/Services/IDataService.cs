using Stat.Api.Models;

namespace Stat.Api.Services;

public interface IDataService
{
    Task<IEnumerable<League>> GetLeagues();
    
    Task<IEnumerable<Match>> GetMatchesByLeagueId(string leagueId);
    
    Task<IEnumerable<Brand>> GetBrands();
    
    Task<IEnumerable<TeamBrand>> GetTeamListByBrandId(string brandId);
    
    Task<IEnumerable<Match>> GetMatchesByBrandId(string leagueId,string brandId);
    
}