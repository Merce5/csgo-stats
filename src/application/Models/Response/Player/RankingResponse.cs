namespace application.Models.Response.Player
{
    public class RankingResponse
    {
        public int RankingPosition { get; set; }
        public string NickName { get; set; }
        public string PlayerImage { get; set; }
        public int TotalKills { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
    }
}