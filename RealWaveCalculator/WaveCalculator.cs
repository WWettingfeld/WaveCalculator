using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.IO;

namespace RealWaveCalculator
{
    public class WaveCalculator
    {
        public void Run()
        {
            var players = GetPlayers();
            var waves = CreateWaves();
            waves = PopulateWaves(waves, players);
        }

        private List<Player> GetPlayers()
        {
            var reader = new StreamReader(File.OpenRead(@"C:\Users\bwett\Downloads\Aftershock Entrants List.csv"));
            reader.ReadLine();

            var players = new List<Player>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                var player = new Player()
                {
                    Name = values[0],
                    EventCount = values.Count(x => x == "1"),
                    IsInMelee = values[1] == "1",
                    IsInPm = values[2] == "1",
                    IsInS4 = values[3] == "1",
                    IsInMeleeDoubles = values[4] == "1",
                    IsInPmDoubles = values[5] == "1",
                    IsInS4Doubles = values[6] == "1",
                    MeleePartner = values[7],
                    PmPartner = values[8],
                    S4Partner = values[9]
                };

                players.Add(player);
            }

            var sortedPlayers = players.OrderByDescending(x => x.EventCount);

            return sortedPlayers.ToList();
        }

        public List<Wave> CreateWaves()
        {
            var waves = new List<Wave>();

            for (var i = 0; i < 4; i++)
            {
                var wave = new Wave()
                {
                    MeleePlayers = new List<Player>(),
                    S4Players = new List<Player>(),
                    PmPlayers = new List<Player>(),
                    MeleeTeams = new List<Team>(),
                    S4Teams = new List<Team>(),
                    PmTeams = new List<Team>()
                };

                waves.Add(wave);
            }

            return waves;
        }

        public List<Wave> PopulateWaves(List<Wave> waves, List<Player> players)
        {
            foreach (var player in players)
            {
                var choosableWaves = GetPossibleWaves(player, waves);

                if (player.IsInMeleeDoubles)
                {
                    var wave = choosableWaves.OrderBy(w => w.MeleeTeams.Count).FirstOrDefault();

                    var team = new Team()
                    {
                        Teammate1 = player,
                        Teammate2 = players.Where(p => p.Name == player.MeleePartner).FirstOrDefault()
                    };

                    if (team.Teammate2 != null)
                    {
                        team.Teammate2.IsInMeleeDoubles = false;

                        wave.MeleeTeams.Add(team);
                        choosableWaves.Remove(wave);
                    }
                }

                if (player.IsInPmDoubles)
                {
                    var wave = choosableWaves.OrderBy(w => w.PmTeams.Count).FirstOrDefault();

                    var team = new Team()
                    {
                        Teammate1 = player,
                        Teammate2 = players.Where(p => p.Name == player.PmPartner).FirstOrDefault()
                    };

                    if (team.Teammate2 != null)
                    {
                        team.Teammate2.IsInPmDoubles = false;

                        wave.PmTeams.Add(team);
                        choosableWaves.Remove(wave);
                    }
                }

                if (player.IsInS4Doubles)
                {
                    var wave = choosableWaves.OrderBy(w => w.S4Teams.Count).FirstOrDefault();

                    var team = new Team()
                    {
                        Teammate1 = player,
                        Teammate2 = players.Where(p => p.Name == player.S4Partner).FirstOrDefault()
                    };

                    if (team.Teammate2 != null)
                    {
                        team.Teammate2.IsInS4Doubles = false;

                        wave.S4Teams.Add(team);
                        choosableWaves.Remove(wave);
                    }
                }

                if (player.IsInMelee)
                {
                    var wave = choosableWaves.OrderBy(w => w.MeleePlayers.Count).FirstOrDefault();
                    wave.MeleePlayers.Add(player);
                }

                if (player.IsInPm)
                {
                    var wave = choosableWaves.OrderBy(w => w.PmPlayers.Count).FirstOrDefault();
                    wave.PmPlayers.Add(player);
                }

                if (player.IsInS4)
                {
                    var wave = choosableWaves.OrderBy(w => w.S4Players.Count).FirstOrDefault();
                    wave.S4Players.Add(player);
                }
            }

            return waves;
        }

        public List<Wave> GetPossibleWaves(Player player, List<Wave> waves)
        {
            var choosableWaves = new List<Wave>();

            foreach (var wave in waves)
            {
                var isInMeleeTeams = wave.MeleeTeams.Where(t => t.Teammate1 == player || t.Teammate2 == player).Any();
                var isInPmTeams = wave.PmTeams.Where(t => t.Teammate1 == player || t.Teammate2 == player).Any();
                var isInS4Teams = wave.S4Teams.Where(t => t.Teammate1 == player || t.Teammate2 == player).Any();

                if (!isInMeleeTeams && !isInPmTeams && !isInS4Teams)
                {
                    choosableWaves.Add(wave);
                }
            }

            return choosableWaves;
        }
    }
}
