using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Models.Response
{
    public class PlayerResponse
    {
        public string NickName { get; set; }
        public string TeamName { get; set; }
        public int TotalGames { get; set; }
        public int WinnedGames { get; set; }
        public int HighestWinStreak { get; set; }
        public int MVPs { get; set; }
        public int HighestScore { get; set; }
        public string PlayerImage { get; set; }
    }
}