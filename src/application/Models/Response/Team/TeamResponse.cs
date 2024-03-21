namespace application.Models.Response.Team
{
    public class TeamResponse
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int WinnedGames { get; set; }
        public int HighestWinStreak { get; set; }
        public string TeamMVP { get; set; }
        public IEnumerable<TeamPlayer> Players { get; set; }
    }
}