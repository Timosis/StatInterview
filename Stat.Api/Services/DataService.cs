using System.Net;
using Newtonsoft.Json;
using Stat.Api.Models;

namespace Stat.Api.Services;

public class DataService : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly string _sasToken;
    private readonly string _requestUri;

    public DataService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _sasToken = configuration.GetValue<string>("SasToken");
        _requestUri = configuration.GetValue<string>("RequestUri");
    }

    public async Task<IEnumerable<League>> GetLeagues()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_requestUri + "leagues.json" + _sasToken);
            var leagues = JsonConvert.DeserializeObject<List<League>>(response);
            return leagues;
        }
        catch (Exception e)
        {
            // Handle HttpRequestException by returning an empty list
            return Enumerable.Empty<League>();
        }
    }
    
    public async Task<IEnumerable<Match>> GetMatchesByLeagueId(string leagueId)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_requestUri + $"leagues/{leagueId}.json" + _sasToken);
            var leagueMatches = JsonConvert.DeserializeObject<IEnumerable<Match>>(response);
            return leagueMatches;
        }
        catch (Exception e)
        {
            // Handle HttpRequestException by returning an empty list
            return Enumerable.Empty<Match>();
        }
    }
    
    public async Task<IEnumerable<Brand>> GetBrands()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_requestUri + $"brands.json" + _sasToken);
            var brands = JsonConvert.DeserializeObject<IEnumerable<Brand>>(response);
            return brands;
        }
        catch (HttpRequestException)
        {
            // Handle HttpRequestException by returning an empty list
            return Enumerable.Empty<Brand>();
        }
    }

    public async Task<IEnumerable<TeamBrand>> GetTeamListByBrandId(string brandId)
    {
        try
        {
            var teamBrandResponse = await _httpClient.GetStringAsync(_requestUri + $"brands/{brandId}.json" + _sasToken);
            var brands = JsonConvert.DeserializeObject<IEnumerable<TeamBrand>>(teamBrandResponse);
            return brands;
        }
        catch (HttpRequestException)
        {
            // Handle HttpRequestException by returning an empty list
            return Enumerable.Empty<TeamBrand>();
        }
    }

    public async Task<IEnumerable<Match>> GetMatchesByBrandId(string leagueId, string brandId)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_requestUri + $"leagues/{leagueId}.json" + _sasToken);
            var matches = JsonConvert.DeserializeObject<IEnumerable<Match>>(response);

            if (!string.IsNullOrEmpty(brandId))
            {
                var brandResponse = await _httpClient.GetStringAsync(_requestUri + $"brands/{brandId}.json" + _sasToken);
                var brand = JsonConvert.DeserializeObject<IEnumerable<TeamBrand>>(brandResponse);

                foreach (var match in matches)
                {
                    var homeTeamBrand = brand.FirstOrDefault(b => b.TeamId == match.HomeTeam.Id);
                    if (homeTeamBrand != null)
                    {
                        match.HomeTeam.Brand = new MatchBrand()
                        {
                            Name = homeTeamBrand.Name,
                            PrimaryColor = homeTeamBrand.PrimaryColor
                        };

                    }

                    var awayTeamBrand = brand.FirstOrDefault(b => b.TeamId == match.AwayTeam.Id);
                    if (awayTeamBrand != null)
                    {
                        match.AwayTeam.Brand = new MatchBrand()
                        {
                            Name = awayTeamBrand.Name,
                            PrimaryColor = awayTeamBrand.PrimaryColor
                        };
                    }
                }
            }
            
            return matches;
        }
        catch (Exception e)
        {
            // Handle HttpRequestException by returning an empty list
            return Enumerable.Empty<Match>();
        }
    }
    
}