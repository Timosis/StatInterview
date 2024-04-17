namespace Stats.Api.Models;

public class Match
{
    public string? Id { get; set; }
    public DateTime Date { get; set; }
    public int GameWeek { get; set; }
    public Team? HomeTeam { get; set; }
    public Team? AwayTeam { get; set; }
    public string? Venue { get; set; }
}