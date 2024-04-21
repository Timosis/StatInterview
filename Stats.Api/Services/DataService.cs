using System.Net;
using Newtonsoft.Json;
using Stats.Api.Models;

namespace Stats.Api.Services;

public class DataService : IDataService
{
    private readonly HttpClient  _client;
    private readonly string? _sasToken;
    private readonly string? _requestUri;

    public DataService(IHttpClientFactory factory, IConfiguration configuration)
    {
        _client = factory.CreateClient("StatClient");
        _sasToken = configuration.GetValue<string>("SasToken");
        _requestUri = configuration.GetValue<string>("RequestUri");
    }

    /// <summary>
    /// Fetches a list of all leagues from the external service.
    /// </summary>
    /// <returns>
    /// A ServiceResponse object containing a collection of League objects.
    /// If the operation is successful, the Success property is true and the Data property contains the leagues.
    /// If the operation fails, the Success property is false and the Message property contains the error message.
    /// </returns>
    public async Task<ServiceResponse<List<League>>> GetLeagues()
    {
        var leagueResponse = new ServiceResponse<List<League>>();
        try
        {
            var leagueData = await _client.GetStringAsync(_requestUri + "leagues.json" + _sasToken);
            leagueResponse.Data = JsonConvert.DeserializeObject<List<League>>(leagueData);
        }
        catch (HttpRequestException ex)
        {
            leagueResponse.Message = $"An error occurred: {ex.Message}";
            leagueResponse.Success = false;
        }
        catch (Exception ex)
        {
            leagueResponse.Message = $"An unexpected error occurred: {ex.Message}";
            leagueResponse.Success = false; 
        }
        
        return leagueResponse;
    }
    
    /// <summary>
    /// Fetches a list of all matches for a given league from the external service.
    /// </summary>
    /// <param name="leagueId">The ID of the league for which to fetch matches.</param>
    /// <returns>
    /// A ServiceResponse object containing a collection of Match objects.
    /// If the operation is successful, the Success property is true and the Data property contains the matches.
    /// If the operation fails, the Success property is false and the Message property contains the error message.
    /// </returns>
    public async Task<ServiceResponse<List<Match>>> GetMatchesByLeagueId(string leagueId)
    {
        var matchResponse = new ServiceResponse<List<Match>>();
        try
        {
            var matchData = await _client.GetStringAsync(_requestUri + $"leagues/{leagueId}.json" + _sasToken);
            matchResponse.Data = JsonConvert.DeserializeObject<List<Match>>(matchData);
        }
        catch (HttpRequestException httpRequestException)
        {
            matchResponse.Message = $"An error occurred: {httpRequestException.Message}";
            matchResponse.Success = false;
        }
        catch (Exception generalException)
        {
            matchResponse.Message = $"An unexpected error occurred: {generalException.Message}";
            matchResponse.Success = false;
        }
        
        return matchResponse;
    }
    
    /// <summary>
    /// Fetches a list of all brands from the external service.
    /// </summary>
    /// <returns>
    /// A ServiceResponse object containing a collection of Brand objects.
    /// If the operation is successful, the Success property is true and the Data property contains the brands.
    /// If the operation fails, the Success property is false and the Message property contains the error message.
    /// </returns>
    public async Task<ServiceResponse<List<Brand>>> GetBrands()
    {
        var brandResponse = new ServiceResponse<List<Brand>>();
        try
        {
            var brandData = await _client.GetStringAsync(_requestUri + "brands.json" + _sasToken);
            brandResponse.Data = JsonConvert.DeserializeObject<List<Brand>>(brandData);
        }
        catch (HttpRequestException httpRequestException)
        {
            brandResponse.Message = $"An error occurred: {httpRequestException.Message}";
            brandResponse.Success = false;
        }
        catch (Exception generalException)
        {
            brandResponse.Message = $"An unexpected error occurred: {generalException.Message}";
            brandResponse.Success = false;
        }

        return brandResponse;
    }

    /// <summary>
    /// Fetches a list of all teams associated with a given brand from the external service.
    /// </summary>
    /// <param name="brandId">The ID of the brand for which to fetch teams.</param>
    /// <returns>
    /// A ServiceResponse object containing a collection of TeamBrand objects.
    /// If the operation is successful, the Success property is true and the Data property contains the teams.
    /// If the operation fails, the Success property is false and the Message property contains the error message.
    /// </returns>
    public async Task<ServiceResponse<List<TeamBrand>>> GetTeamListByBrandId(string brandId)
    {
        var teamBrandResponse = new ServiceResponse<List<TeamBrand>>();
        try
        {
            var teamBrandData = await _client.GetStringAsync(_requestUri + $"brands/{brandId}.json" + _sasToken);
            teamBrandResponse.Data = JsonConvert.DeserializeObject<List<TeamBrand>>(teamBrandData);
        }
        catch (HttpRequestException httpRequestException)
        {
            teamBrandResponse.Message = $"An error occurred: {httpRequestException.Message}";
            teamBrandResponse.Success = false;
        }
        catch (Exception generalException)
        {
            teamBrandResponse.Message = $"An unexpected error occurred: {generalException.Message}";
            teamBrandResponse.Success = false;
        }

        return teamBrandResponse;
    }

    /// <summary>
    /// Fetches a list of all matches for a given league and brand from the external service.
    /// </summary>
    /// <param name="leagueId">The ID of the league for which to fetch matches.</param>
    /// <param name="brandId">The ID of the brand for which to fetch matches.</param>
    /// <returns>
    /// A ServiceResponse object containing a collection of Match objects.
    /// If the operation is successful, the Success property is true and the Data property contains the matches.
    /// If the operation fails, the Success property is false and the Message property contains the error message.
    /// </returns>
    public async Task<ServiceResponse<List<Match>>> GetMatchesByBrandId(string leagueId, string brandId)
    {
        var matchResponse = new ServiceResponse<List<Match>>();
        try
        {
            var response = await _client.GetStringAsync(_requestUri + $"leagues/{leagueId}.json" + _sasToken);
            var matches = JsonConvert.DeserializeObject<List<Match>>(response);

            if (!string.IsNullOrEmpty(brandId))
            {
                var brandResponse = await _client.GetStringAsync(_requestUri + $"brands/{brandId}.json" + _sasToken);
                var brand = JsonConvert.DeserializeObject<List<TeamBrand>>(brandResponse);
                
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

            matchResponse.Data = matches;
            matchResponse.Success = true;
        }
        catch (Exception e)
        {
            matchResponse.Message = $"An error occurred: {e.Message}";
            matchResponse.Success = false;
        }

        return matchResponse;
    }
    
}