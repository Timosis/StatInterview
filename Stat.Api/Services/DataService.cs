using System.Net;
using Newtonsoft.Json;
using Stat.Api.Models;

namespace Stat.Api.Services;

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
    public async Task<ServiceResponse<IEnumerable<League>>> GetLeagues()
    {
        var leagueResponse = new ServiceResponse<IEnumerable<League>>();
        try
        {
            var leagueData = await _client.GetStringAsync(_requestUri + "leagues.json" + _sasToken);
            leagueResponse.Data = JsonConvert.DeserializeObject<List<League>>(leagueData) ?? Enumerable.Empty<League>();
        }
        catch (HttpRequestException ex)
        {
            leagueResponse.Message = $"An error occurred: {ex.Message}";
            leagueResponse.Success = false;
            leagueResponse.Data = Enumerable.Empty<League>();
        }
        catch (Exception ex)
        {
            leagueResponse.Message = $"An unexpected error occurred: {ex.Message}";
            leagueResponse.Success = false;
            leagueResponse.Data = Enumerable.Empty<League>();
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
    public async Task<ServiceResponse<IEnumerable<Match>>> GetMatchesByLeagueId(string leagueId)
    {
        var matchResponse = new ServiceResponse<IEnumerable<Match>>();
        try
        {
            var matchData = await _client.GetStringAsync(_requestUri + $"leagues/{leagueId}.json" + _sasToken);
            matchResponse.Data = JsonConvert.DeserializeObject<IEnumerable<Match>>(matchData) ?? Enumerable.Empty<Match>();
        }
        catch (HttpRequestException httpRequestException)
        {
            matchResponse.Message = $"An error occurred: {httpRequestException.Message}";
            matchResponse.Success = false;
            matchResponse.Data = Enumerable.Empty<Match>();
        }
        catch (Exception generalException)
        {
            matchResponse.Message = $"An unexpected error occurred: {generalException.Message}";
            matchResponse.Success = false;
            matchResponse.Data = Enumerable.Empty<Match>();
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
    public async Task<ServiceResponse<IEnumerable<Brand>>> GetBrands()
    {
        var brandResponse = new ServiceResponse<IEnumerable<Brand>>();
        try
        {
            var brandData = await _client.GetStringAsync(_requestUri + "brands.json" + _sasToken);
            brandResponse.Data = JsonConvert.DeserializeObject<IEnumerable<Brand>>(brandData) ?? Enumerable.Empty<Brand>();
        }
        catch (HttpRequestException httpRequestException)
        {
            brandResponse.Message = $"An error occurred: {httpRequestException.Message}";
            brandResponse.Success = false;
            brandResponse.Data = Enumerable.Empty<Brand>();
        }
        catch (Exception generalException)
        {
            brandResponse.Message = $"An unexpected error occurred: {generalException.Message}";
            brandResponse.Success = false;
            brandResponse.Data = Enumerable.Empty<Brand>();
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
    public async Task<ServiceResponse<IEnumerable<TeamBrand>>> GetTeamListByBrandId(string brandId)
    {
        var teamBrandResponse = new ServiceResponse<IEnumerable<TeamBrand>>();
        try
        {
            var teamBrandData = await _client.GetStringAsync(_requestUri + $"brands/{brandId}.json" + _sasToken);
            teamBrandResponse.Data = JsonConvert.DeserializeObject<IEnumerable<TeamBrand>>(teamBrandData) ?? Enumerable.Empty<TeamBrand>();
        }
        catch (HttpRequestException httpRequestException)
        {
            teamBrandResponse.Message = $"An error occurred: {httpRequestException.Message}";
            teamBrandResponse.Success = false;
            teamBrandResponse.Data = Enumerable.Empty<TeamBrand>();
        }
        catch (Exception generalException)
        {
            teamBrandResponse.Message = $"An unexpected error occurred: {generalException.Message}";
            teamBrandResponse.Success = false;
            teamBrandResponse.Data = Enumerable.Empty<TeamBrand>();
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
    public async Task<ServiceResponse<IEnumerable<Match>>> GetMatchesByBrandId(string leagueId, string brandId)
    {
        var matchResponse = new ServiceResponse<IEnumerable<Match>>();
        try
        {
            var matchData = await _client.GetStringAsync(_requestUri + $"leagues/{leagueId}/brands/{brandId}.json" + _sasToken);
            matchResponse.Data = JsonConvert.DeserializeObject<IEnumerable<Match>>(matchData) ?? Enumerable.Empty<Match>();
        }
        catch (HttpRequestException httpRequestException)
        {
            matchResponse.Message = $"An error occurred: {httpRequestException.Message}";
            matchResponse.Success = false;
            matchResponse.Data = Enumerable.Empty<Match>();
        }
        catch (Exception generalException)
        {
            matchResponse.Message = $"An unexpected error occurred: {generalException.Message}";
            matchResponse.Success = false;
            matchResponse.Data = Enumerable.Empty<Match>();
        }

        return matchResponse;
    }
    
}