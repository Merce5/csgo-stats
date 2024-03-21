namespace csgo_stats.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int WinnerTeam { get; set; }
        public int LoserTeam { get; set; }
        public int MVP { get; set; }
    }
}