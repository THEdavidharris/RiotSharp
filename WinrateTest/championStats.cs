using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinrateTest
{
    class championStats
    {
        public RiotSharp.ChampionEndpoint.Champion champion { get; set; }

        public long gamesPlayed { get; set; }

        public long gamesWon { get; set; }

        private double winrate
        {
            get
            {
                return gamesWon / gamesPlayed;
            }
        }

        public championStats(RiotSharp.ChampionEndpoint.Champion championIn)
        {
            this.champion = championIn;
            this.gamesPlayed = 0;
            this.gamesWon = 0;
        }
    }
}
