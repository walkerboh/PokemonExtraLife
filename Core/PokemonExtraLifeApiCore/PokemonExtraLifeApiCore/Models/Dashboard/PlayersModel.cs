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

        public IEnumerable<Player> AsList => new List<Player>
            {Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8, Player9, Player10};
    }
}