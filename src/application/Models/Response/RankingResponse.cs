using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Models.Response
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