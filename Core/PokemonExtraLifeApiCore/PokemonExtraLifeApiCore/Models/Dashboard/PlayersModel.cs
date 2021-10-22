using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.Models.Dashboard
{
    public class PlayersModel
    {
        public List<Player> Players { get; set; }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player Player3 { get; set; }
        public Player Player4 { get; set; }
        public Player Player5 { get; set; }
        public Player Player6 { get; set; }
        public Player Player7 { get; set; }
        public Player Player8 { get; set; }
        public Player Player9 { get; set; }
        public Player Player10 { get; set; }

        public static PlayersModel Empty =>
            new PlayersModel
            {
                Players = new List<Player>
                {
                    new Player {Id = 1},
                    new Player {Id = 2},
                    new Player {Id = 3},
                    new Player {Id = 4},
                    new Player {Id = 5},
                    new Player {Id = 6},
                    new Player {Id = 7},
                    new Player {Id = 8},
                    new Player {Id = 9},
                    new Player {Id = 10},
                }
            };
    }
}