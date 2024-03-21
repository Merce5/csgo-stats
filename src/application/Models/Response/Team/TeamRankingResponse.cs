namespace application.Models.Response.Team
{
    public class TeamRankingResponse
    {
        public int Ranking { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public int WinnedGames { get; set; }
        public IEnumerable<string> TeamMembers { get; set; }
    }
}