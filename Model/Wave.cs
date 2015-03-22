using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Wave
    {
        public List<Player> PmPlayers { get; set; }
        public List<Player> MeleePlayers { get; set; }
        public List<Player> S4Players { get; set; }
        public List<Team> PmTeams { get; set; }
        public List<Team> MeleeTeams { get; set; }
        public List<Team> S4Teams { get; set; }
    }
}
